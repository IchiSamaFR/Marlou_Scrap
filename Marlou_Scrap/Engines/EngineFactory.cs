using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarlouScrapper.Engines
{
    internal class EngineFactory
    {
        public static IEngine GetEngine(string engineName)
        {
            switch (engineName.ToLower())
            {
                case "json":return new EngineJson();
                default: throw new ArgumentException("Engine " + engineName + " not found");
            }
        }
    }
}
