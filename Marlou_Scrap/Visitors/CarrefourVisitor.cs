using HtmlAgilityPack;
using MarlouScrap.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marlou_Scrap.Visitors
{
    public class CarrefourVisitor : AHTMLVisitor
    {
        public const string HTML_BEER_PAGE = @"https://www.auchan.fr/vins-bieres-alcool/bieres-futs-cidres/ca-n071201?page={page}";
        public class BeerStats
        {
            public string Url { get; set; }
            public string Name { get; set; }
            public string Product { get; set; }
            public string TopicDate { get; set; }
        }

        /// <summary>
        /// Return all beers / cidres
        /// </summary>
        /// <param name="pages"></param>
        /// <returns></returns>
        public List<BeerStats> GetBeers(int pages)
        {
            List<BeerStats> beers = new List<BeerStats>();
            for (int i = 0; i < pages; i++)
            {
                beers.AddRange(GetBeers(GetHtml(HTML_BEER_PAGE.Replace("{page}", i.ToString()))));
            }
            return beers;
        }

        protected List<BeerStats> GetBeers(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var lst = new List<BeerStats>();
            var topicList = FindFirstClass(doc.DocumentNode, "list__container");

            foreach (HtmlNode? node in FindAttributes(topicList, "class", "list__item"))
            {
                BeerStats beer = new BeerStats();
                beer.Name = Clean(FindFirstClass(node, "product-thumbnail__description")?.OuterHtml);
                beer.Product = "Bière / Cidre";
                lst.Add(beer);
            }
            return lst;
        }
    }
}
