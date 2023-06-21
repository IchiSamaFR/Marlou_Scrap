using MarlouScrapper.Engines;
using System.Xml.Linq;

namespace MarlouScrapper.Contexts
{
    internal class ContextParam
    {
        public ContextParam(XElement e)
        {
            Engine = EngineFactory.GetEngine(e.Attribute("engine")?.Value ?? string.Empty);
            XPath = e.Attribute("xpath")?.Value ?? string.Empty;
            To = e.Attribute("to")?.Value ?? string.Empty;
        }

        public IEngine Engine { get; }
        public string XPath { get; }
        public string To { get; }
    }
}