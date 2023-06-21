using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace MarlouScrapper.Contexts
{
    internal class ContextReplace
    {
        public ContextReplace(XElement e)
        {
            Pattern = new Regex(e.Attribute("pattern")?.Value ?? "(.*)");
            By = e.Attribute("by")?.Value ?? string.Empty;
        }

        public Regex Pattern { get; }
        public string By { get; }

        public string Replace(string str)
        {
            return Pattern.Replace(str, By);
        }
    }
}