using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MarlouScrapper.Extensions;

namespace MarlouScrapper.Contexts
{
    internal class Context
    {
        private readonly XDocument _xDocument;
        private readonly XElement _root;
        private Dictionary<string, string>? _properties = null;
        private List<string>? _proxies = null;
        private List<ContextScrapper>? _scrappers = null;

        public Dictionary<string, string> Properties
        {
            get
            {
                _properties ??= _root.Dictionnary("property", e => e.Attribute("name")?.Value ?? string.Empty, e => e.Value);
                return _properties;
            }
        }

        public List<string> Proxies
        {
            get
            {
                _proxies ??= _root.Childs("proxy", proxy => proxy.Attribute("url")?.Value.WithProperties(this) ?? string.Empty);
                return _proxies;
            }
        }

        internal List<ContextScrapper> Scrappers
        {
            get
            {
                _scrappers ??= _root.Childs("scrapper", scrapper => ContextScrapper.Build(scrapper, this));
                return _scrappers;
            }
        }

        public Context(XDocument xDocument)
        {
            _xDocument = xDocument;
            _root = _xDocument.Root ?? new XElement("root");
        }
        public Context(Dictionary<string, string>? properties)
        {
            _xDocument = new XDocument();
            _root = new XElement("root");
            _properties = properties;
        }

        public static Context LoadFromXML(string xmlName)
        {
            return new Context(XDocument.Load(xmlName));
        }

        public string? ApplyProperties(string? str)
        {
            if (str == null)
            {
                return null;
            }
            foreach (var prop in Properties)
            {
                str = str.Replace("{" + prop.Key + "}", prop.Value);
            }
            return str;
        }

        public void AddProperty(string key, string? value)
        {
            if (value == null)
            {
                return;
            }
            _properties ??= new Dictionary<string, string>();
            _properties.Add(key, value);
        }

        internal void RemoveProperty(string key)
        {
            if (_properties == null || !_properties.ContainsKey(key))
            {
                return;
            }
            _properties.Remove(key);
        }
    }
}
