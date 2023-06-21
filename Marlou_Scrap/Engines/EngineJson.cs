using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MarlouScrapper.Engines
{
    internal class EngineJson : IEngine
    {
        public string? XPath(string content, string xpath)
        {
            //var json = JsonSerializer.Deserialize<JsonObject>(content);
            var json = JsonNode.Parse(content);
            foreach (var p in xpath.Split('.'))
            {
                json = json?[p];
            }
            return json?.ToString();
        }
    }
}
