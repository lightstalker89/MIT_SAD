using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algodat_Lists
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            DoubleLinkedList dll = new DoubleLinkedList();
            dll.CreateDLList(numbers);
            dll.PrintForward();
            dll.PrintReverse();
            dll.Remove(dll.Head);
            dll.InsertAfter(dll.Tail, 30);
            dll.PrintNode(dll.Head);
            dll.PrintNode(dll.Tail);
        }
    }
}
