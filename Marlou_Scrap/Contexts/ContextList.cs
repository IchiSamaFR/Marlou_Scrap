using MarlouScrapper.Extensions;
using System.Xml.Linq;

namespace MarlouScrapper.Contexts
{
    internal class ContextList
    {
        public ContextWeb Web { get; private set; } = null!;
        public ContextSelect Select { get; private set; } = null!;
        public string? LoopName { get; private set; }
        public int? LoopStart { get; private set; }
        public int? LoopEnd { get; private set; }

        private ContextList()
        {

        }
        internal static ContextList Build(XElement e, Context context)
        {
            var loop = e.Attribute("loop")?.Value.Split(':');
            return new ContextList()
            {
                LoopName = loop?[0],
                LoopStart = loop?[1].ToInt(),
                LoopEnd = loop?[2].ToInt(),
                Web = BuildWeb(e, context),
                Select = new ContextSelect(e),
            };
        } 

        private static ContextWeb BuildWeb(XElement e, Context context)
        {
            var web = e.Elements().FirstOrDefault(child => child.Name == "web");
            return web == null ? throw new ArgumentNullException("balise web absente dans un list") : ContextWeb.Build(web, context);
        }
    }
}