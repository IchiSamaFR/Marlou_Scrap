using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MarlouScrapper.Contexts;
using MarlouScrapper.Extensions;

namespace MarlouScrapper
{
    internal class Scrapper
    {
        public Scrapper(Context context)
        {
            HTML = HTML.Build(context);
        }

        public HTML HTML { get; private set; }

        public static Scrapper Build(Context context)
        {
            return new Scrapper(context);
        }

        public JsonArray Action(ContextScrapper contextScrapper)
        {
            var res = new JsonArray();
            var localContext = new Context(new Dictionary<string, string>());
            foreach (var init in contextScrapper.Init)
            {
                init.LocalContext = localContext;
                var response = HTML.Send(init);
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.Text();
                }
            }
            foreach (var list in contextScrapper.Pages)
            {
                list.Web.LocalContext = localContext;
                if (list.LoopName != null)
                {
                    for(var loopIndex  = list.LoopStart; loopIndex <= list.LoopEnd; loopIndex++)
                    {
                        localContext.AddProperty(list.LoopName, loopIndex.ToString());
                        var response = HTML.Send(list.Web);
                        var doc = new HtmlDocument();
                        doc.LoadHtml(response.Content.Text());
                        res.Add( HTML.AnalyseNode(doc.DocumentNode, list.Select));
                        localContext.RemoveProperty(list.LoopName);
                    }
                }
                else
                {
                    var response = HTML.Send(list.Web);
                    var doc = new HtmlDocument();
                    doc.LoadHtml(response.Content.Text());
                    res.Add(HTML.AnalyseNode(doc.DocumentNode, list.Select));
                }
            }
            return res;

        }

    }
}
