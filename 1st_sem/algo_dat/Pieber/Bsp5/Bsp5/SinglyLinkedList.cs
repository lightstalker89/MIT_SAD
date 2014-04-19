using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp5
{
    class SinglyLinkedList<T>
    {
        public Node Head { get; private set; }

        public Node InsertFirst(T data)
        {
            return this.InsertAfter(null, data);
        }

        public Node InsertAfter(Node node, T data)
        {
            Node newNode = new Node();

            newNode.Data = data;

            if (node == null)
            {
                newNode.Next = this.Head;
                this.Head = newNode;
            }
            else
            {
                newNode.Next = node.Next;
                node.Next = newNode;
            }

            return newNode;
        }

        public void RemoveFirst()
        {
            this.RemoveNext(null);
        }

        public void RemoveNext(Node node)
        {
            if (node == null &&
                this.Head != null)
            {
                this.Head = this.Head.Next;
            }
            else if (node != null &&
                node.Next != null)
            {
                node.Next = node.Next.Next;
            }
        }

        public void List(Node node)
        {
            if (node == null)
            {
                return;
            }

            Console.Write(" {0} ", node.Data);
            this.List(node.Next);
        }

        [System.Diagnostics.DebuggerDisplay("{Data}")]
        public class Node
        {
            public T Data { get; set; }

            public Node Next { get; set; }
        }
    }
}
