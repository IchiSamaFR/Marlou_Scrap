using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;
using MarlouScrapper.Extensions;
using static System.Net.Mime.MediaTypeNames;

namespace MarlouScrapper.Contexts
{
    internal class ContextWeb
    {
        public string Url { get; private set; } = string.Empty;
        public HttpMethod Method { get; private set; } = HttpMethod.Get;
        public Context? LocalContext { get; set; }
        public Dictionary<string, string> Headers { get; private set; } = new Dictionary<string, string>();
        public List<Cookie> Cookies { get; private set; }

        internal static ContextWeb Build(XElement e, Context context)
        {
            return new ContextWeb()
            {
                Url = e.Attribute("url")?.Value.WithProperties(context) ?? string.Empty,
                Method = new HttpMethod(e.Attribute("method")?.Value.ToUpper() ?? "GET"),
                Headers = e.Dictionnary("header", e => e.Attribute("name")?.Value.WithProperties(context) ?? string.Empty, e => e.Value.WithProperties(context)),
                Cookies = e.Childs("cookie", e =>
                {
                    return new Cookie(e.Attribute("name")?.Value.WithProperties(context),
                           e.Attribute("value")?.Value.WithProperties(context),
                           e.Attribute("path")?.Value.WithProperties(context),
                           e.Attribute("domain")?.Value.WithProperties(context));
                })
            };
        }

        public static implicit operator HttpRequestMessage(ContextWeb contextWeb)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(contextWeb.Url.WithProperties(contextWeb.LocalContext))
            };
            return httpRequestMessage;
        }
    }



}
