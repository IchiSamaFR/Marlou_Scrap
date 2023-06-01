using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarlouScrap.Tools
{
    public class MathTool
    {
        public static string ALCOOL_DEGREE = "([0-9]*[.,]*[0-9]*%)";
        public static string ALCOOL_NUMBER = "([0-9]*)x";
        public static string ALCOOL_CONTAINS = "([0-9]*\\.*\\,*[0-9]*)((?:cl)|(?:l))";
    }
}
