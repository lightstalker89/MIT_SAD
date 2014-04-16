using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algodat_Hashing
{
    public class CArray
    {
        private List<string> textArray { get; set; }

        public CArray()
        {
            this.textArray = new List<string>();
        }

        public bool SearchForString(string str)
        {
            return this.textArray.Contains(str);
        }

        public void AddString(string str) 
        {
            this.textArray.Add(str);
        }
    }
}
