using MarlouScrap.Models.Marlou;
using MarlouScrap.Tools;
using MarlouScrap.Visitors;
using Newtonsoft.Json;

namespace MarlouScrap
{
    public partial class Program
    {
        public static void Main()
        {
            StartInfos();
        }
        public static void StartInfos()
        {
            var lst = LoadJson<AlcoolStats>("file.json");

            //Console.WriteLine(string.Join("\n", lst.Distinct(new AlcoolComparer()).Where(b => b.Degree > 0 && !b.Name.ToLower().Contains("crème")).Distinct().OrderBy(b => b.Price / (b.Degree * b.Contain * b.Quantity)).Take(10).Select(b => b.Debug())));
        }

        public static void StartScrap()
        {
            var tmp = new AuchanVisitor();
            var lst = tmp.GetBeers();
            lst.AddRange(tmp.GetWines());
            lst.AddRange(tmp.GetAperitifs());
            lst.AddRange(tmp.GetChampagnes());
            //Console.WriteLine(string.Join("\n", lst.Select(b => b.Debug())));

            SaveJson(lst, "file.json");
        }
        public static List<T> LoadJson<T>(string path)
        {
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
        private static void SaveJson<T>(List<T> lst, string path)
        {
            string json = JsonConvert.SerializeObject(lst, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        private static void OnProgress(float value)
        {
            int progressSize = 20;
            int progress = (int)MathF.Round(value * progressSize);
            Console.SetCursorPosition(0, 0);
            Console.Write("[" + new string('#', progress) + new string('.', progressSize - progress) + "]");
            Console.Write((value * 100).ToString("0") + "%");
        }
    }
}