using System;
using System.Web;

namespace HemenTanitim.Models
{
    public class MyModule1 : IHttpModule
    {
        /// <summary>
        /// Buradaki temizleme kodunuzun Web.config dosyasında bu modülü
        /// ve kullanabilmek için IIS'de kaydettirmeniz gerekecektir. Daha fazla bilgi için
        /// şu bağlantıya gidin: https://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Üyeleri

        public void Dispose()
        {
            //yapılandırmanız gerekecektir.
        }

        public void Init(HttpApplication context)
        {
            // Aşağıda, LogRequest olayını nasıl işleyebileceğiniz ve ona ilişkin özel günlük kaydı uygulamasını nasıl sağlayacağınız
            // konusunda bir örnek verilmiştir
            context.LogRequest += new EventHandler(OnLogRequest);
        }

        #endregion

        public void OnLogRequest(Object source, EventArgs e)
        {
            //özel günlük kaydı mantığı buraya girilebilir
        }
    }
}
