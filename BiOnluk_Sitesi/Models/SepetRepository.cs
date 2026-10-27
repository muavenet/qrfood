using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
namespace HemenTanitim.Models
{
    public class SepetRepository
    {
        readonly string _connString = ConfigurationManager.ConnectionStrings["DB150408011842Entities"].ConnectionString;
       
        public IEnumerable<SepetAdetUrunAdi> GetAllMessages(string uid)
        {
            var messages = new List<SepetAdetUrunAdi>();

            using (var connection = new SqlConnection(_connString))
            {
                int count = 0;

                connection.Open();

                using (SqlCommand command = new SqlCommand(@"SELECT [Id],[UrunAdi],[Fiyat] FROM dbo.[Sepet] where KullaniciId='" + uid + "'", connection))
                {
                    command.Notification = null;

                    var dependency = new SqlDependency(command);

                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        messages.Add(item: new SepetAdetUrunAdi {Id = (int)reader["Id"], UrunAdi = (string)reader["UrunAdi"], Adet = count++, Fiyat = (decimal)reader["Fiyat"]});
                    }
                }
            }

            return messages;
        }

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SepetHub.Show();
            }
        }
    }
}