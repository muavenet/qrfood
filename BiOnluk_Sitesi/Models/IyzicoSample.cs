using Iyzipay;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using System.Diagnostics;


namespace HemenTanitim.Models
{
    public class IyzicoSample
    {

        protected void PrintResponse<T>(T resource)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            Trace.WriteLine(JsonConvert.SerializeObject(resource, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
        
        }
    }
}