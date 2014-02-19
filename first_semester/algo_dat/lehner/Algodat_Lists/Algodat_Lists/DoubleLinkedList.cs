using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;

namespace Algodat_Lists
{
    public class DoubleLinkedList
    {
        public Node Head { get; private set; }
        public Node Tail { get; private set; }

        public Node Current { get; private set; }

        public void CreateDLList(int[] numbers)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                Node n = new Node();
                if (i == 0)
                {
                    this.Head = n;
                    this.Tail = n;
                    n.Previous = null;
                    n.Next = null;
                    n.Data = numbers[i];
                }
                else
                {
                    this.InsertAfter(this.Tail, numbers[i]);
                }
            }
        }

        public void InsertAfter(Node n, int data)
        {
            Node newNode = new Node();
            newNode.Data = data;
            newNode.Next = n.Next;
            newNode.Previous = n;

            if (n.Next != null)
            {
                n.Next.Previous = newNode;
            }

            n.Next = newNode;

            if (newNode.Next == null)
            {
                this.Tail = newNode;
            }
        }

        public void InsertBefore(Node n, int data)
        {
            Node newNode = new Node();
            newNode.Data = data;
            newNode.Next = n;
            newNode.Previous = n.Previous;

            if (n.Previous != null)
            {
                n.Previous.Next = newNode;
            }

            n.Previous = newNode;

            if (newNode.Previous == null)
            {
                this.Head = newNode;
            }
        }

        public void Remove(Node n)
        {
            if (n.Previous == null)
            {
                this.Head = n.Next;
                this.Head.Previous = null;
            } 
            else if (n.Next == null)
            {
                this.Tail = n.Previous;
                this.Tail.Next = null;
            }
            else
            {
                n.Next.Previous = n.Previous;
                n.Previous.Next = n.Next;
            }
        }

        public void PrintReverse()
        {
            this.Current = this.Tail;
            while (this.Current != null)
            {
                Console.Write("{0}", this.Current.Data);
                if (this.Current.Previous != null)
                {
                    Console.Write(", ");
                }
                else
                {
                    Console.WriteLine();
                }
                this.StepBack();
            }
        }

        public void PrintForward()
        {
            this.Current = this.Head;
            while (this.Current != null)
            {
                Console.Write("{0}", this.Current.Data);
                if (this.Current.Next != null)
                {
                    Console.Write(", ");
                }
                else
                {
                    Console.WriteLine();
                }
                this.StepForward();
            }
        }

        public Node StepBack()
        {
            if (this.Current == null)
            {
                this.Current = this.Tail;
            }
            else
            {
                this.Current = this.Current.Previous;
            }
            return this.Current;
        }

        public Node StepForward()
        {
            if (this.Current == null)
            {
                this.Current = this.Head;
            }
            else
            {
                this.Current = this.Current.Next;
            }
            return this.Current;
        }

        public void PrintNode(Node n) 
        {
            Console.WriteLine(n.Data);
        }

    }
}
