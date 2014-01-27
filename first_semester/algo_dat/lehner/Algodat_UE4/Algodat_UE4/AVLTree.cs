using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algodat_UE4
{
    public class AVLTree : BinaryTree
    {
        private int GetLeftDepth(Node n, int count)
        {
            if (n == null)
            {
                return count;
            }
            if (n.LeftNode != null)
            {
                count = this.GetLeftDepth(n.LeftNode, count);
            }

            return count += 1;
        }

        private int GetRightDepth(Node n, int count)
        {
            if (n == null)
            {
                return count;
            }
            if (n.RightNode != null)
            {
                count = this.GetRightDepth(n.RightNode, count);
            }

            return count += 1;
        }

        public int LeftHeight()
        {
            return this.GetLeftDepth(this.RootNode, 0);
        }

        public int RightHeight()
        {
            return this.GetRightDepth(this.RootNode, 0);
        }

        public int BalanceFactor()
        {
            return this.LeftHeight() - this.RightHeight();
        }
    }
}
