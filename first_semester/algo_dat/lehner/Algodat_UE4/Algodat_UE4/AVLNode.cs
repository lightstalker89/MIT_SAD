using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algodat_UE4
{
    public class AVLNode
    {
        public int Value { get; set; }
        public AVLNode LeftNode { get; set; }
        public AVLNode RightNode { get; set; }
        public AVLNode ParentNode { get; set; }

        public AVLNode(int value)
        {
            this.Value = value;
        }

        public int GetBalance()
        {
            int leftHeight = (this.LeftNode == null) ? 0 : this.LeftNode.Height();
            int rightHeight = (this.RightNode == null) ? 0 : this.RightNode.Height();

            return rightHeight - leftHeight;
        }
        public int Height()
        {
            int leftHeight = (this.LeftNode == null) ? 0 : this.LeftNode.Height();
            int rightHeight = (this.RightNode == null) ? 0 : this.RightNode.Height();

            return 1 + Math.Max(leftHeight, rightHeight);
        }

    }
}
