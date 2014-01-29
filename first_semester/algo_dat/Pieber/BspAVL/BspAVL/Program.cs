using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BspAVL
{
    public class AVL
    {
        static void Main(string[] args)
        {
            int[] array = CreateAndInitArray(10, 20);
            //array = new int[] { 8, 11, 16, 18, 14, 5, 0, 13, 1, 7 };
            //array = new int[] { 8, 11, 16, 18, 14, 20, 22 };
            //array = new int[] { 22, 20, 18, 16, 14, 10 };
            //array = new int[] { 20, 16, 18, 26, 22, 24 };
            //array = new int[] { 16, 20, 18, 22, 26, 24 };
            //array = new int[] { 15, 16, 8, 11, 5, 1, 5, 17, 9 };
            //array = new int[] { 14, 13, 2, 0, 3, 4, 6, 11, 15, 8 };
            //array = new int[] { 6, 11, 4, 0, 13, 5, 1, 2, 15, 14 };
            //array = new int[] { 10, 3, 1, 11, 16, 6, 7, 11, 8, 13 };
            //array = new int[] { 2, 4, 19, 3, 6, 16, 8, 20, 11, 7, 5 };
            AVLTree tree = new AVLTree();

            foreach (int item in array)
            {
                Console.WriteLine("Adding {0}", item);
                tree.Insert(item);
                Console.WriteLine();
                PrintTree(tree);
                Console.WriteLine();
            }

            //tree.Delete(6);
            //Console.WriteLine();
            //PrintTree(tree);
            //Console.WriteLine();

            //Console.ReadLine();

            array = CreateAndInitArray(100000, 1000000);
            tree.Clear();

            Timing t = new Timing();

            Console.Write("Inserting {0} random elements into AVL tree... ", array.Length);

            t.StartTime();

            foreach (int item in array)
            {
                tree.Insert(item);
            }

            t.StopTime();

            Console.WriteLine("done; took {1}", array.Length, t.Result().ToString());

            Console.Write("Inserting same {0} random elements into binary tree... ", array.Length);

            BinaryTree btree = new BinaryTree();

            t.StartTime();

            foreach (int item in array)
            {
                btree.Insert(item);
            }

            t.StopTime();

            Console.WriteLine("done; took {1}", array.Length, t.Result().ToString());

            int[] search = new int[100];

            Array.Copy(array, array.Length - 100, search, 0, 100);
            Node n;
            int count = 0;

            Console.Write("Searching last 100 elements in AVL tree... ");

            t.StartTime();

            foreach (int item in search)
            {
                n = tree.Find(item);

                if (n != null &&
                    n.Value == item)
                {
                    ++count;
                }
            }

            t.StopTime();

            Console.WriteLine("done; took {0} (ticks: {1})", t.Result().ToString(), t.Result().Ticks);

            Console.Write("Searching same 100 elements in binary tree... ");

            t.StartTime();

            foreach (int item in search)
            {
                n = btree.Find(item);

                if (n != null &&
                    n.Value == item)
                {
                    ++count;
                }
            }

            t.StopTime();

            Console.WriteLine("done; took {0} (ticks: {1})", t.Result().ToString(), t.Result().Ticks);

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = i + 1;
            }

            Console.Write("Inserting {0} sorted elements into AVL tree... ", array.Length);

            tree.Clear();

            t.StartTime();

            foreach (int item in array)
            {
                tree.Insert(item);
            }

            t.StopTime();

            Console.WriteLine("done; took {1}", array.Length, t.Result().ToString());

            Console.Write("Inserting same {0} sorted elements into binary tree... ", array.Length);

            btree = new BinaryTree();

            t.StartTime();

            foreach (int item in array)
            {
                btree.Insert(item);
            }

            t.StopTime();

            Console.WriteLine("done; took {1}", array.Length, t.Result().ToString());

            count = 0;

            Console.Write("Searching last 100 elements in AVL tree... ");

            t.StartTime();

            foreach (int item in search)
            {
                n = tree.Find(item);

                if (n != null &&
                    n.Value == item)
                {
                    ++count;
                }
            }

            t.StopTime();

            Console.WriteLine("done; took {0} (ticks: {1})", t.Result().ToString(), t.Result().Ticks);

            Console.Write("Searching same 100 elements in binary tree... ");

            t.StartTime();

            foreach (int item in search)
            {
                n = btree.Find(item);

                if (n != null &&
                    n.Value == item)
                {
                    ++count;
                }
            }

            t.StopTime();

            Console.WriteLine("done; took {0} (ticks: {1})", t.Result().ToString(), t.Result().Ticks);

            Console.ReadLine();
        }

        public static void PrintTree(AVLTree tree)
        {
            Node[] nodes = new Node[] { tree.Root }, tmp;
            int i = 0, j = 0, max = Math.Max(tree.Root.LeftDepth, tree.Root.RightDepth) + 1;

            Console.WindowWidth = Console.LargestWindowWidth - 15;
            Console.WindowHeight = Console.LargestWindowHeight - 5;
            Console.BufferWidth = 1000;
            Console.BufferHeight = 1000;

            do
            {
                tmp = nodes;
                nodes = new Node[(i + 1) * 2];
                i = -1;

                if (j < max)
                {
                    ++j;
                    Console.Write(string.Empty.PadRight(Math.Max(0, 10 * (int)Math.Pow(2, (max - j - 1)) - 5)));
                }

                foreach (Node n in tmp)
                {
                    if (n == null)
                    {
                        Console.Write("| {0,3} {1,2} |", string.Empty, string.Empty);
                        nodes[++i] = null;
                        nodes[++i] = null;
                    }
                    else
                    {
                        Console.Write("| {0,3} {1,2} |", (int)n.Value, n.BalanceFactor);

                        if (n.Left != null)
                        {
                            nodes[++i] = n.Left;
                        }
                        else
                        {
                            nodes[++i] = null;
                        }

                        if (n.Right != null)
                        {
                            nodes[++i] = n.Right;
                        }
                        else
                        {
                            nodes[++i] = null;
                        }
                    }

                    Console.Write(string.Empty.PadRight(Math.Max(0, 10 * ((int)Math.Pow(2, max - j) - 1))));
                }

                Console.WriteLine();
            } while (!nodes.All<Node>(n => n == null));
        }

        public static int[] CreateAndInitArray(int maxCount, int maxValue)
        {
            int[] array = new int[maxCount];

            if (maxValue < 0)
            {
                throw new ArgumentException("Value need to be greater or equal to 0", "maxValue");
            }

            Random r = new Random();

            for (int i = 0; i < maxCount; ++i)
            {
                array[i] = r.Next(maxValue);
            }

            return array;
        }
    }
}
