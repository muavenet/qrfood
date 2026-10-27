using HemenTanitim.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace HemenTanitim.YardimciElemanlar
{

  
    public static class YardimciElemanlar
    {
        public static string TrimFrontIfLongerThan(this string value, int maxLength)
        {
            if (value.Length > maxLength)
            {
                return value.Substring(0, maxLength);
            }

            return value;
        }

        public static string FirstCharToUpper(this string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");

            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }

        public static string turkceSeoUrlTemizle(this string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                url = url.Replace("-", " ");

                url = url.Replace("i", "İ");
                url = url.Replace("ı", "I");

                url = url.ToUpper();

                url = url.Trim();

                

                return url;
            }
            return "";
        }

        public static string turkceSeoUrl(this string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                url = url.Replace(" ", "-");

                url = url.ToLower();

                url = url.Trim();

                return url;
            }
            return "";
        }

        public static string ToSeoUrl(this string url)
        {

            if (!string.IsNullOrEmpty(url))
            {
                url = url.Replace("ş", "s");
                url = url.Replace("Ş", "s");
                url = url.Replace("İ", "i");
                url = url.Replace("ı", "i");
                url = url.Replace("ö", "o");
                url = url.Replace("Ö", "o");
                url = url.Replace("ü", "u");
                url = url.Replace("Ü", "u");
                url = url.Replace("Ç", "");
                url = url.Replace("ç", "c");
                url = url.Replace("ğ", "g");
                url = url.Replace("Ğ", "g");
                url = url.Replace(" ", "-");
                url = url.Replace("---", "-");
                url = url.Replace("?", "");
                url = url.Replace("/", "");
                url = url.Replace(".", "");
                url = url.Replace("'", "");
                url = url.Replace("#", "");
                url = url.Replace("%", "");
                url = url.Replace("&", "");
                url = url.Replace("*", "");
                url = url.Replace("!", "");
                url = url.Replace("@", "");
                url = url.Replace("+", "");
                url = url.Replace(":", "");

                url = url.ToLower();
                url = url.Trim();

                // tüm harfleri küçült
                string encodedUrl = (url ?? "").ToLower();

                // & ile " " yer değiştirme
                encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");

                // " " karakterlerini silme
                encodedUrl = encodedUrl.Replace("'", "");

                // geçersiz karakterleri sil
                //encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-");

                // tekrar edenleri sil
                encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");

                // karakterlerin arasına tire koy
                encodedUrl = encodedUrl.Trim('-');

                return encodedUrl;
            }
            return "";
        }

        public static string ToTemizle(this string url)
        {

            if (!string.IsNullOrEmpty(url))
            {
                url = url.Replace("ş", "s");
                url = url.Replace("Ş", "s");
                url = url.Replace("İ", "i");
                url = url.Replace("I", "ı");
                url = url.Replace("ö", "o");
                url = url.Replace("Ö", "o");
                url = url.Replace("ü", "u");
                url = url.Replace("Ü", "u");
                url = url.Replace("Ç", "");
                url = url.Replace("ç", "c");
                url = url.Replace("ğ", "g");
                url = url.Replace("Ğ", "g");
                url = url.Replace(" ", "-");
                url = url.Replace("---", "-");
                url = url.Replace("?", "");
                url = url.Replace("/", "");
                url = url.Replace(".", "");
                url = url.Replace("'", "");
                url = url.Replace("#", "");
                url = url.Replace("%", "");
                url = url.Replace("&", "");
                url = url.Replace("*", "");
                url = url.Replace("!", "");
                url = url.Replace("@", "");
                url = url.Replace("+", "");
                url = url.Replace(":", "");


                url = url.Trim();

                // tüm harfleri küçült
                string encodedUrl = (url ?? "").ToLower();

                // & ile " " yer değiştirme
                encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");

                // " " karakterlerini silme
                encodedUrl = encodedUrl.Replace("'", "");

                // geçersiz karakterleri sil
                //encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-");

                // tekrar edenleri sil
                encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");

                // karakterlerin arasına tire koy
                encodedUrl = encodedUrl.Trim('-');

                return encodedUrl;
            }
            return "";
        }
    }
}