using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    public abstract class Tree
    {
        public TreeNode Root { get; set; }
        public abstract void Insert(int key, int value);
        // public abstract bool Delete(TreeNode node);
        public abstract bool Delete(int key);
        public abstract bool Search(int key, out int value);
        public abstract void Output(OutputType type ,TreeNode root);
    }
}
