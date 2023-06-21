using System.Xml.Linq;
using MarlouScrapper.Extensions;

namespace MarlouScrapper.Contexts
{
    internal class ContextScrapper
    {

        public string Name { get; private set; } = string.Empty;
        public List<ContextWeb> Init { get; private set; } = new List<ContextWeb>();
        public ContextSelect Select { get; private set; } = null!;
        public List<ContextList> List { get; private set; } = null!;

        internal static ContextScrapper Build(XElement e, Context context)
        {
            return new ContextScrapper()
            {
                Name = e.Attribute("name")?.Value ?? string.Empty,
                Init = e.Childs("init", init => ContextWeb.Build(init, context)),
                List = e.Childs("list", list => ContextList.Build(list, context)),
            };
        }

        private ContextScrapper() { }
    }



}
