using HtmlAgilityPack;
using MarlouScrap.WebTools;
using MarlouScrap.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarlouScrap.Visitors
{
    public class AuchanVisitor : AHTMLVisitor
    {
        public override List<Cookie> Cookies
        {
            get
            {
                return new List<Cookie>()
                {
                    new Cookie("lark-journey", "f1cb5fe9-359e-4840-86ad-c64ab77e657f", "/", "www.auchan.fr"),
                };
            }
        }
        public const string HTML_BEER_PAGE = @"https://www.auchan.fr/vins-bieres-alcool/bieres-futs-cidres/ca-n071201?page={page}";

        public class BeerStats
        {
            public string Url { get; set; }
            public string Name { get; set; }
            public string Brand { get; set; }
            public string ProductType { get; set; }
            public decimal Degree { get; set; }
            public int Quantity { get; set; }
            public decimal Contain { get; set; }
            public decimal Price { get; set; }

            public string Debug()
            {
                if(Quantity > 1)
                {
                    return $"{Brand} {Name}\n{Price.ToString("0.00")}e - {Degree}° - {Quantity}x{Contain}L";
                }

                return $"{Brand} {Name}\n{Price.ToString("0.00")}e - {Degree}° - {Contain}L";
            }
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
                decimal d;

                var desc = FindFirstClass(node, "product-thumbnail__description");
                var brand = FindFirstAttribute(desc, "itemprop", "brand");
                if(brand == null)
                {
                    continue;
                }
                beer.Brand = Clean(brand?.InnerHtml);
                beer.Name = Clean(FindFirstClass(node, "product-thumbnail__description")?.InnerHtml.Substring(brand?.OuterHtml.Length ?? 0));
                beer.ProductType = "Bière / Cidre";

                var degreeMatch = Regex.Match(beer.Name, MathTool.ALCOOL_DEGREE).Value;
                if (decimal.TryParse(degreeMatch.Replace("%", "").Replace(".", ","), out d))
                {
                    beer.Degree = d;
                    beer.Name = beer.Name.Replace($" {degreeMatch}", "");
                }

                var price = FindFirstAttribute(node, "itemprop", "price");
                if (decimal.TryParse(price?.GetAttributes("content").FirstOrDefault()?.Value.Replace(".", ","), out d))
                {
                    beer.Price = d;
                }

                var att = FindFirstClass(FindFirstClass(node, "product-thumbnail__attributes"), "product-attribute")?.InnerHtml;
                if (att != null)
                {
                    var qtt = Regex.Match(att, MathTool.ALCOOL_NUMBER).Groups[1].Value;
                    if (int.TryParse(qtt, out int i))
                    {
                        beer.Quantity = i;
                    }
                    else
                    {
                        beer.Quantity = 1;
                    }

                    var contMatch = Regex.Match(att.Replace("ccl", "cl"), MathTool.ALCOOL_CONTAINS);
                    var amount = contMatch.Groups[1].Value;
                    var volume = contMatch.Groups[2].Value;
                    if (decimal.TryParse(amount.Replace(".", ","), out d))
                    {
                        beer.Contain = volume == "cl" ? d * 0.01m : d;
                    }
                }

                lst.Add(beer);
            }
            return lst;
        }
    }
}
