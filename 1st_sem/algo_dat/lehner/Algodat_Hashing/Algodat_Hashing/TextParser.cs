using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algodat_Hashing
{
    public class TextParser
    {
        private string TextString { get; set; }
        public TextParser(string text)
        {
            this.TextString = text;
        }

        public string[] ParseText()
        {
            return this.TextString.Split(new char[] { ' ', '.', ',', ';', ':', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

    }
}
