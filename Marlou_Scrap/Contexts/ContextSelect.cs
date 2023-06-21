using System.Xml.Linq;
using MarlouScrapper.Extensions;

namespace MarlouScrapper.Contexts
{
    internal class ContextSelect
    {
        public ContextSelect(XElement e)
        {
            Query = e.Attribute("query")?.Value;
            To = e.Attribute("to")?.Value ?? "select-" + Guid.NewGuid().ToString();
            Childs = e.Childs("select", e => new ContextSelect(e));
            Text = e.Childs("text", e => new ContextText(e));
            IsSingle = e.BooleanAttribute("single");
        }

        public string? Query { get; }
        public string To { get; }
        public List<ContextSelect> Childs { get; }
        public List<ContextText> Text { get; }
        public bool IsSingle { get; }
    }
}