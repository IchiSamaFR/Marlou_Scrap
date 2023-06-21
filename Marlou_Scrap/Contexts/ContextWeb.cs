using System.Linq;
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
        public string? Payload { get; private set; } = string.Empty;
        public HttpMethod Method { get; private set; } = HttpMethod.Get;
        public Context? LocalContext { get; set; }
        public Dictionary<string, string> Headers { get; private set; } = new Dictionary<string, string>();
        public List<ContextParam> Params { get; private set; } = new List<ContextParam>();
        public string? PayloadType { get; private set; }

        internal static ContextWeb Build(XElement e, Context context)
        {
            return new ContextWeb()
            {
                Url = e.Attribute("url")?.Value.WithProperties(context) ?? string.Empty,
                Method = new HttpMethod(e.Attribute("method")?.Value.ToUpper() ?? "GET"),
                Payload = (e.Text().StripSpaces(true)) == string.Empty ? null : e.Text().WithProperties(context).StripSpaces(e.BooleanAttribute("stripspaces")),
                PayloadType= e.Attribute("payload")?.Value,
                Headers = e.Dictionnary("header", e => e.Attribute("name")?.Value.WithProperties(context) ?? string.Empty, e => e.Value.WithProperties(context)),
                Params = e.Childs("param", param=>new ContextParam(param)),
            };

        }
        public static implicit operator HttpRequestMessage(ContextWeb contextWeb)
        {
            
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(contextWeb.Url.WithProperties(contextWeb.LocalContext)),
                Method = contextWeb.Method,
                Content = Content(contextWeb),
            };
            foreach (var kv in contextWeb.Headers)
            {
                Console.WriteLine(kv.Key+":"+ kv.Value.WithProperties(contextWeb.LocalContext));
                httpRequestMessage.Headers.Add(kv.Key, kv.Value.WithProperties(contextWeb.LocalContext));
            }
            return httpRequestMessage;
        }

        public static HttpContent? Content(ContextWeb contextWeb)
        {
            if (contextWeb.Payload == null)
            {
                return null;
            }
            switch (contextWeb.PayloadType) {
                case "form":return Form(contextWeb.Payload.WithProperties(contextWeb.LocalContext));
                default: return new StringContent(contextWeb.Payload.WithProperties(contextWeb.LocalContext));
            }
        }
        public static HttpContent? Form(string payload)
        {
            //return new FormUrlEncodedContent(new Dictionary<string, string>() { { "test", "ok" } });
            ByteArrayContent byteArrayContent = new ByteArrayContent(Encoding.ASCII.GetBytes(payload));
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

           // byteArrayContent.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            return byteArrayContent;
        }
    }



}
