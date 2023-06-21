using System.Text.RegularExpressions;
using System.Web;
using MarlouScrapper.Contexts;

namespace MarlouScrapper.Extensions
{
    internal static class StringExtension
    {
        public static string WithProperties(this string str, Context? context)
        {
            return context?.ApplyProperties(str) ?? str;
        }
        public static string StripSpaces(this string str, bool active)
        {
            return active ? Regex.Replace(str, @"\s", "") : str;
        }
        public static string Trim(this string str, bool active)
        {
            return active ? str.Trim() : str;
        }
        public static string HTMLDecode(this string str, bool active)
        {
            return active ? HttpUtility.HtmlDecode(str) : str;
        }
        public static string Replace(this string str, List<ContextReplace> contextReplace)
        {
            foreach (var replace in contextReplace)
            {
                str = replace.Replace(str);
            }
            return str;
        }

        public static int ToInt(this string str)
        {
            return int.Parse(str);
        }
    }

}
