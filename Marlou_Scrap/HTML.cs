using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using System.Text.Json.Nodes;
using System.Transactions;
using MarlouScrapper.Contexts;
using MarlouScrapper.Extensions;

namespace MarlouScrapper
{
    internal class HTML
    {
        private HttpClient _client;

        public HTML(WebProxy? webProxy)
        {
            _client = new HttpClient(new HttpClientHandler
            {
                Proxy = webProxy,
                UseCookies = true
            });
        }

        public static HTML Build(Context context)
        {
            return new HTML(context.Proxies.Count > 0 ? new WebProxy(context.Proxies[new Random().Next(0, context.Proxies.Count)]) : null);
        }

        public HttpResponseMessage Send(ContextWeb contextWeb)
        {
            Console.WriteLine(">>" + contextWeb.Method.ToString() + " " + contextWeb.Url.WithProperties(contextWeb.LocalContext));
            var response = _client.Send(contextWeb);
            Console.WriteLine("<<" + response.StatusCode);
            return response;
        }

        public void Analyse(HtmlNode node, ContextSelect select, JsonObject o)
        {
            if (select.IsSingle)
            {
                AnalyseNode(node.QuerySelector(select.Query), select, o);
            }
            else
            {
                var suba = new JsonArray();
                foreach (var subnode in node.QuerySelectorAll(select.Query))
                {
                    suba.Add(AnalyseNode(subnode, select));
                }
                o.Add(select.To, suba);
            }
        }
        public JsonObject AnalyseNode(HtmlNode node, ContextSelect select, JsonObject? o = null)
        {
            o ??= new JsonObject();
            foreach (var text in select.Text)
            {
                o.Add(text.To, text.Extract(node));
            }
            foreach (var subSelect in select.Childs)
            {
                Analyse(node, subSelect, o);
            }
            return o;
        }
    }
}
