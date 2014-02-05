using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beispiel1A_1B
{
    public class Stack<T>
    {
        IList<T> collection;
        const int STACKSIZE = 500;

        public Stack()
        {
            collection = new List<T>(500);
        }

        /// <summary>
        /// Entfernt das oberste Objekt aus Stack und gibt es zurück.
        /// </summary>
        public T Pop()
        {
            try
            {
                T o = collection.Last();
                collection.Remove(o);
                return o;
            }
            catch (InvalidOperationException e)
            {
                throw;
            }
        }

        /// <summary>
        /// Fügt ein Objekt am Anfang von Stack ein.
        /// </summary>
        public void Push(T o)
        {
            try
            {
                if (!(collection.Count == STACKSIZE))
                {
                    collection.Add(o);
                }
                else
                {
                    throw new OutOfMemoryException();
                }

            }
            catch (NotSupportedException e)
            {
                throw;
            }
            catch (OutOfMemoryException e)
            {
                throw;
            }
        }

        /// <summary>
        /// Gibt das oberste Objekt von Stack zurück, ohne es zu entfernen.
        /// </summary>
        public T Peek()
        {
            try
            {
                T o = collection.LastOrDefault();
                return o;
            }
            catch (ArgumentNullException e)
            {
                throw;
            }
        }
    }
}
