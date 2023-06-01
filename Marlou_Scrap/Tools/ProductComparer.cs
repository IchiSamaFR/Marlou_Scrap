using MarlouScrap.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MarlouScrap.Visitors.AuchanVisitor;

namespace MarlouScrap.Tools
{
    public class ProductComparer : IEqualityComparer<ProductStats>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(ProductStats x, ProductStats y)
        {
            return x.Name == y.Name && x.Brand == y.Brand;
        }

        // If Equals() returns true for a pair of objects
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(ProductStats product)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(product, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hashProductName = product.Name == null ? 0 : product.Name.GetHashCode();

            //Get hash code for the Code field.
            int hashProductCode = product.Brand.GetHashCode();

            //Calculate the hash code for the product.
            return hashProductName ^ hashProductCode;
        }
    }
}
