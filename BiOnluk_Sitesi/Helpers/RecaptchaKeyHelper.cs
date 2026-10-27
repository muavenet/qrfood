using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemenTanitim.Helpers
{
    internal class RecaptchaKeyHelper
    {
        internal static string ParseKey(string key)
        {
            if (key.StartsWith("{") && key.EndsWith("}"))
            {
                return System.Configuration.ConfigurationManager.AppSettings[key.Trim().Substring(1, key.Length - 2)];
            }

            return key;
        }
    }
}