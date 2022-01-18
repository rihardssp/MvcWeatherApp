using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Extensions
{
    /// <summary>
    /// So many problems with existing solutions, and the proper ones are hidden in ASP.net not .net..
    /// </summary>
    public static class IDictionaryExtension
    {
        public static string ToUrlQuery(this IDictionary<string, string> dictionary)
        {
            var data = dictionary
                .Where(e => !string.IsNullOrEmpty(e.Key) && !string.IsNullOrEmpty(e.Value))
                .Select(e => $"{Uri.EscapeDataString(e.Key)}={Uri.EscapeDataString(e.Value)}");
            return "?" + string.Join("&", data);
        }
    }
}
