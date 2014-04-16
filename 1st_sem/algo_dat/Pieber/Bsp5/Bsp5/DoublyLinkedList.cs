using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp5
{
    class DoublyLinkedList<T> : IEnumerable
    {
        public Node Head { get; private set; }

        public Node Tail { get; private set; }

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
                newNode.Previous = null;
                this.Head = newNode;
                this.Tail = this.Head;
            }
            else
            {
                newNode.Next = node.Next;
                newNode.Previous = node;
                node.Next = newNode;
            }

            if (newNode.Next == null)
            {
                this.Tail = newNode;
            }

            return newNode;
        }

        public Node InsertBefore(Node node, T data)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            Node newNode = new Node();

            newNode.Data = data;

            newNode.Next = node;
            newNode.Previous = node.Previous;
            if (node.Previous != null)
            {
                node.Previous.Next = newNode;
            }
            node.Previous = newNode;

            if (newNode.Previous == null)
            {
                this.Head = newNode;
            }

            return newNode;
        }

        public void RemoveAt(Node node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            if (node == this.Head)
            {
                this.Head = this.Head.Next;
                this.Head.Previous = null;
            }
            else
            {
                if (node.Previous != null)
                {
                    node.Previous.Next = node.Next;
                }

                if (node.Next != null)
                {
                    node.Next.Previous = node.Previous;
                }
            }
        }

        public void ListForward(Node node)
        {
            if (node == null)
            {
                return;
            }

            Console.Write(" {0} ", node.Data);
            this.ListForward(node.Next);
        }

        public void ListReverse(Node node)
        {
            if (node == null)
            {
                return;
            }

            Console.Write(" {0} ", node.Data);
            this.ListReverse(node.Previous);
        }

        public IEnumerator GetEnumerator()
        {
            foreach (T item in this.Forward)
            {
                yield return item;
            }
        }

        public IEnumerable Forward
        {
            get
            {
                Node n = this.Head;

                while (n != null)
                {
                    yield return n.Data;

                    n = n.Next;
                }
            }
        }

        public IEnumerable Backward
        {
            get
            {
                Node n = this.Tail;

                while (n != null)
                {
                    yield return n.Data;

                    n = n.Previous;
                }
            }
        }

        [System.Diagnostics.DebuggerDisplay("{Data}")]
        public class Node
        {
            public T Data { get; set; }

            public Node Next { get; set; }

            public Node Previous { get; set; }
        }

    }
}
