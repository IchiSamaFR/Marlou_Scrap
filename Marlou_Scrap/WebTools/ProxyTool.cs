using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace MarlouScrap.WebTools
{
    public abstract class ProxyTool
    {
        private const string FILTER_PATH = "Resources/http_proxies.txt";

        public static WebProxy GetProxy()
        {
            var lst = File.ReadAllLines(FILTER_PATH).ToList();

            var ip = lst[new Random().Next(0, lst.Count)];
            WebProxy proxy = new WebProxy();
            proxy.Address = new Uri("http://" + ip);

            return proxy;
        }
    }
}
