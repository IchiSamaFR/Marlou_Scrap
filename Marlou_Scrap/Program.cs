// See https://aka.ms/new-console-template for more information
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using MarlouScrapper;
using MarlouScrapper.Contexts;
using MarlouScrapper.Engines;
using MarlouScrapper.Extensions;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Xml.Linq;


public class Program
{
    public static void Main()
    {
        string xmlFilename = Path.GetFullPath("config.xml");
        Console.WriteLine(xmlFilename);
        var context = Context.LoadFromXML(xmlFilename);
        var scrapper = Scrapper.Build(context);

        var doc = new HtmlDocument();
        doc.LoadHtml(File.ReadAllText("content.txt"));
        Console.WriteLine(scrapper.HTML.AnalyseNode(doc.DocumentNode, context.Scrappers[0].Pages[0].Select));
        context.Scrappers[0].Pages[0].Web.LocalContext = new Context(new Dictionary<string, string>() { { "page", "1" } });


        Console.WriteLine(scrapper.Action(context.Scrappers[0]));
    }
}

