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
            //int[] randNumbers = new int[10];

            //for (int i = 0; i < 10; i++)
            //{
            //    randNumbers[i] = r.Next(1000);
            //}

            //Array.Sort(randNumbers);

           int[] randNumbers = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

            BinaryTree tree = new BinaryTree();
            tree.CreateTree(randNumbers);
            Console.WriteLine("BINARY");
            tree.Print();
            Console.WriteLine("++++++++++++++++++++++++");
            if (tree.Find(randNumbers[5]))
            {
                Console.WriteLine("Found!!");
            } 
            else 
            {
                Console.WriteLine("Not Found!!");
            }

            tree.Remove(randNumbers[5]);
            if (tree.Find(randNumbers[5]))
            {
                Console.WriteLine("Found!!");
            }
            else
            {
                Console.WriteLine("Not Found!!");
            }
            tree.Print();
            Console.WriteLine("++++++++++++++++++++++++");

            Console.WriteLine("AVL");
            AVLTree avlTree = new AVLTree();
            avlTree.CreateAVLTree(randNumbers);
            avlTree.Print();
        }
    }
}
