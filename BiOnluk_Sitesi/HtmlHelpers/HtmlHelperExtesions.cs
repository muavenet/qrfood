using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HemenTanitim.HtmlHelpers
{
    public class HtmlHelperExtesions
    {
        
    }
    public static class ImageHelper
    {
        public static MvcHtmlString Image(this HtmlHelper helper, string src, string altText, string height)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", src);
            builder.MergeAttribute("alt", altText);
            builder.MergeAttribute("height", height);
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static string FirstCharToUpper(this string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");

            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }
    }
}