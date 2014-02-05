using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beispiel1A_1B
{
    public class Queue<T>
    {
        private IList<T> collection;
        private const int QUEUESIZE = 500;

        /// <summary>
        /// Initialisiert eine neue, leere Instanz der Queue-Klasse, 
        /// die anfänglich über die Standardkapazität verfügt und den Standardzuwachsfaktor verwendet.
        /// </summary>
        public Queue()
        {
            collection = new List<T>();
        }

        /// <summary>
        /// Entfernt das Objekt am Anfang von Queue und gibt es zurück.
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            try
            {
                T o = collection.First();
                collection.Remove(o);
                return o;
            }
            catch (NotSupportedException e)
            {
                throw;
            }
            catch (InvalidOperationException e)
            {
                throw;
            }
        }

        /// <summary>
        /// Fügt am Ende der Queue ein Objekt hinzu.
        /// </summary>
        /// <param name="o"></param>
        public void Enqueue(T o)
        {
            try
            {
                if (!(collection.Count() == QUEUESIZE))
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
        }

        /// <summary>
        /// Gibt das Objekt am Anfang von Queue zurück, ohne es zu entfernen.
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            T o = collection.FirstOrDefault();
            return o;
        }
    }
}
