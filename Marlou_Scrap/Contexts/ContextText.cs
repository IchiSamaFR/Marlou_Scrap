using HtmlAgilityPack;
using MarlouScrapper.Extensions;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace MarlouScrapper.Contexts
{
    internal class ContextText
    {
        public ContextText(XElement e)
        {
            Regex = new Regex(e.Attribute("regex")?.Value ?? "(.*)");
            RegexGroup = e.Attribute("regexgroup")?.Value ?? "1";
            IsTrim = e.BooleanAttribute("trim");
            IsHtmlDecode = e.BooleanAttribute("htmldecode");
            To = e.Attribute("to")?.Value ?? "text-" + Guid.NewGuid().ToString();
            Empty = e.Attribute("empty")?.Value ?? string.Empty;
            Replace = e.Childs("replace", replace => new ContextReplace(replace));
        }

        public Regex Regex { get; }
        public string RegexGroup { get; }
        public bool IsTrim { get; }
        public bool IsHtmlDecode { get; }
        public string To { get; }
        public string Empty { get; }
        public List<ContextReplace> Replace { get; }

        public string Extract(HtmlNode e)
        {
            return Regex
                .Matches(string.Join("", e.SelectNodes("text()").Select(node => node.InnerText)))
                .FirstOrDefault(m => true)?
                .Groups[RegexGroup]?.Value
                .HTMLDecode(IsHtmlDecode)
                .Trim(IsTrim)
                .Replace(Replace) ?? Empty;
        }
    }
}