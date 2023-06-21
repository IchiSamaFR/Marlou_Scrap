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

string xmlFilename = Path.GetFullPath("config.xml");
Console.WriteLine(xmlFilename);
var context = Context.LoadFromXML(xmlFilename);
var scrapper = Scrapper.Build(context);

//scrapper.Action(context.Scrappers[0]);
var doc = new HtmlDocument();
doc.LoadHtml(File.ReadAllText("content.txt"));
Console.WriteLine(scrapper.HTML.AnalyseNode(doc.DocumentNode, context.Scrappers[0].List[0].Select));
context.Scrappers[0].List[0].Web.LocalContext = new Context(new Dictionary<string, string>() { { "page", "1" } });
Console.WriteLine(scrapper.Action(context.Scrappers[0]));
//var response = scrapper.HTML.Send(context.Scrappers[0].List[0].Web);
//Console.WriteLine(response.Content.Text());
var e = EngineFactory.GetEngine("json");
var content = @"{  ""Date"": ""2019-08-01T00:00:00"",  ""Temperature"": 25,  ""Summary"": ""Hot"",  ""DatesAvailable"": [    ""2019-08-01T00:00:00"",    ""2019-08-02T00:00:00""  ],  ""TemperatureRanges"": {      ""Cold"": {          ""High"": 20,          ""Low"": -10      },      ""Hot"": {          ""High"": 60,          ""Low"": 20      }  }}";
Console.WriteLine(e.XPath(content, "TemperatureRanges.Cold.High"));

