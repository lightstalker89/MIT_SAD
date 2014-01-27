using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algodat_UE4
{
    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();
            int[] randNumbers = new int[10];

            for (int i = 0; i < 10; i++)
            {
                randNumbers[i] = r.Next(100);
            }

            //Array.Sort(randNumbers);

            int[] test = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

            BinaryTree tree = new BinaryTree();
            tree.CreateTree(randNumbers);
            tree.Print();
            tree.Delete(randNumbers[5]);
            tree.Print();

            AVLTree avlTree = new AVLTree();
            avlTree.CreateTree(randNumbers);
            Console.WriteLine("Balance factor: {0}", avlTree.BalanceFactor());
        }
    }
}
