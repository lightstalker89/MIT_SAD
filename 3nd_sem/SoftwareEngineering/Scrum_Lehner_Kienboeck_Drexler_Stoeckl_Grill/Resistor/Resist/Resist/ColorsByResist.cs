using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Resist
{
    public class ColorsByResist : IColorCalculator
    {

        public System.Windows.Media.Brush[] GetColorsByResist(double resist)
        {
            System.Windows.Media.Brush[] array = new System.Windows.Media.Brush[5];
            string value = resist.ToString();

            while(value.Length < 5)
            {
                value = "0" + value; 
            }

            int index = 0;
            foreach (char item in value)
            {
                
                switch (item)
                {
                    case '0':
                        array[index] = Brushes.Black;
                        break;
                    case '1':
                        array[index] = Brushes.Brown;
                        break;
                    case '2':
                        array[index] = Brushes.Red;
                        break;
                    case '3':
                        array[index] = Brushes.Orange;
                        break;
                    case '4':
                        array[index] = Brushes.Yellow;
                        break;
                    case '5':
                        array[index] = Brushes.Green;
                        break;
                    case '6':
                        array[index] = Brushes.Blue;
                        break;
                    case '7':
                        array[index] = Brushes.Purple;
                        break;
                    case '8':
                        array[index] = Brushes.Gray;
                        break;
                    case '9':
                        array[index] = Brushes.White;
                        break;
                    default:
                        break;
                }
                ++index;
            }
            return array;
        }
    }
}
