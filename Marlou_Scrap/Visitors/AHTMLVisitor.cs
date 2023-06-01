using HtmlAgilityPack;
using MarlouScrap.WebTools;
using System.Net;
using System.Web;

namespace MarlouScrap.Visitors
{
    public abstract class AHTMLVisitor
    {
        public virtual List<Cookie> Cookies { get; } = new List<Cookie>();
        protected string GetHtml(string url, bool useProxy = false)
        {
            var httpClientHandler = new HttpClientHandler();

            if (useProxy)
            {
                httpClientHandler.Proxy = ProxyTool.GetProxy();
            }
            httpClientHandler.UseCookies = true;
            foreach (var cookie in Cookies)
            {
                httpClientHandler.CookieContainer.Add(cookie);
            }
            var client = new HttpClient(httpClientHandler);
            var content = client.GetStringAsync(url).Result;
            return content;
        }
        protected string Clean(string value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            return HttpUtility.HtmlDecode(value.Replace("\n", "").Replace("\r", "").Replace("\t", "").Trim());
        }

        protected static List<HtmlNode> FindAttributes(HtmlNode node, string attribute, string value)
        {
            var lst = new List<HtmlNode>();
            if (node == null)
            {
                return lst;
            }

            if (node.Attributes[attribute]?.Value.Contains(value) == true)
            {
                lst.Add(node);
            }

            foreach (HtmlNode child in node.ChildNodes)
            {
                HtmlNode result = FindFirstAttribute(child, attribute, value);
                if (result != null)
                {
                    lst.Add(result);
                }
            }

            return lst;
        }
        protected static HtmlNode FindFirstClass(HtmlNode node, string nodeClass)
        {
            return FindFirstAttribute(node, "class", nodeClass);
        }
        protected static HtmlNode FindFirstId(HtmlNode node, string nodeClass)
        {
            return FindFirstAttribute(node, "id", nodeClass);
        }
        protected static HtmlNode FindFirstAttribute(HtmlNode node, string attribute, string value = "")
        {
            if (node == null)
            {
                return null;
            }

            if (node.Attributes[attribute]?.Value.Contains(value) == true)
            {
                return node;
            }

            foreach (HtmlNode child in node.ChildNodes)
            {
                HtmlNode result = FindFirstAttribute(child, attribute, value);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
