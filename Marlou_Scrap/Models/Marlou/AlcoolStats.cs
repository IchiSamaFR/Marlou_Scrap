using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarlouScrap.Models.Marlou
{
    public class AlcoolStats
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string ProductType { get; set; }
        public decimal Degree { get; set; }
        public int Quantity { get; set; }
        public decimal Contain { get; set; }
        public decimal Price { get; set; }

        public string Debug()
        {
            if (Quantity > 1)
            {
                return $"{Brand} {Name}\n{Price.ToString("0.00")}e - {Degree}° - {Quantity}x{Contain}L + {ProductType}";
            }

            return $"{Brand} {Name}\n{Price.ToString("0.00")}e - {Degree}° - {Contain}L + {ProductType}";
        }
    }
}
