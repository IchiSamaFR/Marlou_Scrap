using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MarlouScrapper.Extensions
{
    internal static class HTTPContentExtension
    {
        public static string Text(this HttpContent response)
        {
            return new StreamReader(response.ReadAsStream(), Encoding.UTF8).ReadToEnd();
        }
    }
}
