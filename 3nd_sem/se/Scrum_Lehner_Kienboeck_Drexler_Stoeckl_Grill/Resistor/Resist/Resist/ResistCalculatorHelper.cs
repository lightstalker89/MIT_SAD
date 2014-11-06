using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Resist
{
    public class ResistCalculatorHelper : IResistCalculator
    {
        enum Colors
        {
            Black = 0,
            Brown = 1,
            Red = 2,
            Orange = 3,
            Yellow = 4,
            Green = 5,
            Blue = 6,
            Magenta = 7,
            Gray = 8,
            White = 9

        }
        public ResistCalculatorHelper()
        {

        }

        public double GetResist(Brush c1, Brush c2, Brush c3, Brush c4, Brush c5)
        {
            String firstThreeValues = "";
            firstThreeValues += getValueByColor(c1);
            firstThreeValues += getValueByColor(c2);
            firstThreeValues += getValueByColor(c3);

            Double value = Convert.ToDouble(firstThreeValues);

            return value = value * getMultiplicatorByColor(c4);
        }

        public Double getMultiplicatorByColor(Brush b)
        {
            if (b == Brushes.Brown)
            {
                return 10;
            }
            else if (b == Brushes.Black)
            {
                return 1;
            }
            else if (b == Brushes.Red)
            {
                return 100;
            }
            else if (b == Brushes.Orange)
            {
                return 1000;
            }
            else if (b == Brushes.Yellow)
            {
                return 10000;
            }
            else if (b == Brushes.Green)
            {
                return 100000;
            }
            else if (b == Brushes.Blue)
            {
                return 1000000;
            }
            else if (b == Brushes.Violet)
            {
                return 10000000;
            }
            else if (b == Brushes.Gold)
            {
                return 0.1;
            }
            else if (b == Brushes.Silver)
            {
                return 0.01;
            }
            else
            {
                throw new Exception("Not allowed");
            }
        }

        public String getValueByColor(Brush b)
        {
            if (b == Brushes.Black)
            {
                return "0";
            }
            else if (b == Brushes.Brown)
            {
                return "1";
            }
            else if (b == Brushes.Red)
            {
                return "2";
            }
            else if (b == Brushes.Orange)
            {
                return "3";
            }
            else if (b == Brushes.Yellow)
            {
                return "4";
            }
            else if (b == Brushes.Green)
            {
                return "5";
            }
            else if (b == Brushes.Blue)
            {
                return "6";
            }
            else if (b == Brushes.Violet)
            {
                return "7";
            }
            else if (b == Brushes.Gray)
            {
                return "8";
            }
            else if (b == Brushes.White)
            {
                return "9";
            }
            else
            {
                throw new Exception("Not allowed");
            }

        }
    }
}
