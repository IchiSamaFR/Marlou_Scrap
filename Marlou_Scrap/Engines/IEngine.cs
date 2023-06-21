using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarlouScrapper.Engines
{
    internal interface IEngine
    {
        string? XPath(string content, string xpath);
    }
}
