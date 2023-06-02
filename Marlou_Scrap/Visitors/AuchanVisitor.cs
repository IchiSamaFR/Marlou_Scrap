using HtmlAgilityPack;
using MarlouScrap.Models.Marlou;
using MarlouScrap.Tools;
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
    public class AuchanVisitor : AHTMLVisitor, IMarlouVisitor
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
        public const string HTML_BEER_PAGE = @"https://www.auchan.fr/vins-bieres-alcool/bieres-futs-cidres/ca-n071201?page={page}"; // 10
        public const string HTML_WINE_PAGE = @"https://www.auchan.fr/vins-bieres-alcool/vins/ca-n0709?page={page}"; // 13
        public const string HTML_APERITIF_PAGE = @"https://www.auchan.fr/vins-bieres-alcool/aperitifs-spiritueux/ca-n0707?page={page}"; // 10
        public const string HTML_CHAMPAGNE_PAGE = @"https://www.auchan.fr/vins-bieres-alcool/champagnes-vins-effervescents/ca-n0710?page={page}"; // 4

        public const string PRODUCT_BEER = "Bière / Cidre";
        public const string PRODUCT_WINE = "Vin";
        public const string PRODUCT_APERITIF = "Aperitif";
        public const string PRODUCT_CHAMPAGNE = "Champagne";

        public List<AlcoolStats> GetBeers(int pages = 10)
        {
            var tmp = GetProducts(HTML_BEER_PAGE, pages);
            tmp.ForEach(p => p.ProductType = PRODUCT_BEER);
            return tmp;
        }

        public List<AlcoolStats> GetWines(int pages = 13)
        {
            var tmp = GetProducts(HTML_WINE_PAGE, pages);
            tmp.ForEach(p => p.ProductType = PRODUCT_WINE);
            return tmp;
        }

        public List<AlcoolStats> GetAperitifs(int pages = 10)
        {
            var tmp = GetProducts(HTML_APERITIF_PAGE, pages);
            tmp.ForEach(p => p.ProductType = PRODUCT_APERITIF);
            return tmp;
        }

        public List<AlcoolStats> GetChampagnes(int pages = 4)
        {
            var tmp = GetProducts(HTML_CHAMPAGNE_PAGE, pages);
            tmp.ForEach(p => p.ProductType = PRODUCT_CHAMPAGNE);
            return tmp;
        }


        protected List<AlcoolStats> GetProducts(string path, int pages)
        {
            List<AlcoolStats> product = new List<AlcoolStats>();
            for (int i = 0; i < pages; i++)
            {
                product.AddRange(GetScrappedProducts(GetHtml(path.Replace("{page}", i.ToString()))));
            }
            return product;
        }
        protected List<AlcoolStats> GetScrappedProducts(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var lst = new List<AlcoolStats>();
            var topicList = FindFirstClass(doc.DocumentNode, "list__container");

            foreach (HtmlNode? node in FindAttributes(topicList, "class", "list__item"))
            {
                AlcoolStats beer = new AlcoolStats();
                decimal d;

                var desc = FindFirstClass(node, "product-thumbnail__description");
                var brand = FindFirstAttribute(desc, "itemprop", "brand");
                if(brand == null)
                {
                    continue;
                }
                beer.Brand = Clean(brand?.InnerHtml);
                beer.Name = Clean(FindFirstClass(node, "product-thumbnail__description")?.InnerHtml.Substring(brand?.OuterHtml.Length ?? 0));

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
