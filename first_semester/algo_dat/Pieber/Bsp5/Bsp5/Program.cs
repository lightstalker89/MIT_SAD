using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp5
{
    class Program
    {
        static void Main(string[] args)
        {
            SinglyLinkedList<int> sll = new SinglyLinkedList<int>();
            SinglyLinkedList<int>.Node sNode;

            sNode = sll.InsertFirst(5);
            sNode = sll.InsertAfter(sNode, 6);
            sNode = sll.InsertAfter(sNode, 1);
            sNode = sll.InsertAfter(sNode, 90);
            sNode = sll.InsertAfter(sNode, 36);
            sNode = sll.InsertAfter(sNode, 12);
            sNode = sll.InsertAfter(sNode, 42);
            sNode = sll.InsertAfter(sNode, 96);
            sNode = sll.InsertAfter(sNode, 7);
            sll.RemoveFirst();
            sll.RemoveNext(sll.Head.Next.Next);

            sll.List(sll.Head);
            Console.WriteLine();

            DoublyLinkedList<int> dll = new DoublyLinkedList<int>();
            DoublyLinkedList<int>.Node dNode;

            dNode = dll.InsertFirst(8);
            dNode = dll.InsertAfter(dNode, 29);
            dNode = dll.InsertAfter(dNode, 15);
            dNode = dll.InsertAfter(dNode, 74);
            dll.InsertBefore(dll.Tail.Previous.Previous, 999);
            dNode = dll.InsertAfter(dNode, 20);
            dNode = dll.InsertAfter(dNode, 36);
            dNode = dll.InsertAfter(dNode, 40);
            dll.InsertBefore(dll.Head, 765);
            dll.RemoveAt(dll.Head.Next.Next.Next);

            Console.WriteLine();
            Console.WriteLine("List forward:");
            dll.ListForward(dll.Head);
            Console.WriteLine();

            Console.WriteLine("List reverse:");
            dll.ListReverse(dll.Tail);
            Console.WriteLine();

            Console.WriteLine("Forward iterator:");
            foreach (int item in dll.Forward)
            {
                Console.Write(" {0} ", item);
            }
            Console.WriteLine();

            Console.WriteLine("Backward iterator:");
            foreach (int item in dll.Backward)
            {
                Console.Write(" {0} ", item);
            }
            Console.WriteLine();

            Console.ReadLine();
        }
    }
}
