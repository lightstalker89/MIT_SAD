using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algodat_UE4
{
    public class Node
    {
        public int Value { get; private set; }
        public Node LeftNode { get; set; }
        public Node RightNode { get; set; }


        public Node(int value)
        {
            this.Value = value;
        }
    }
}
