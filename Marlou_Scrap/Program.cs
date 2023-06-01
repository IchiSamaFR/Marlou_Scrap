

using Marlou_Scrap.Visitors;

namespace MarlouScrap
{
    public partial class Program
    {
        public static void Main()
        {
            Start();
        }
        public static void Start()
        {
            var tmp = new CarrefourVisitor();
            Console.WriteLine(string.Join("\n, ", tmp.GetBeers(1).Select(b => b.Name)));
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