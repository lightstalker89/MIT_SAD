//-----------------------------------------------------------------------
// <copyright file="Queue.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Beispiel1A_1B
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// My Queue implementation
    /// </summary>
    /// <typeparam name="T">type of the items</typeparam>
    public class Queue<T>
    {
        private IList<T> collection;
        private int queueSize;

        /// <summary>
        /// Initialisiert eine neue, leere Instanz der Queue-Klasse, 
        /// die anfänglich über die Standardkapazität verfügt und den Standardzuwachsfaktor verwendet.
        /// </summary>
        /// <param name="size">Maximum items in the queue</param>
        public Queue(int size)
        {
            this.queueSize = size;
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
            catch (NotSupportedException)
            {
                throw;
            }
            catch (InvalidOperationException)
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
                if (collection.Count < this.queueSize)
                {
                    collection.Add(o);
                }
                else
                {
                    throw new OutOfMemoryException();
                }
            }
            catch (NotSupportedException)
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

        /// <summary>
        /// Representing the items within the queue as a string
        /// </summary>
        /// <returns>Queue items as a string</returns>
        public string MyToString()
        {
            string values = string.Empty;

            if (this.collection != null && this.collection.Count > 0)
            {
                do
                {
                    values += this.Dequeue().ToString() + " ";
                } while (this.collection.Count != 0);
            }

            return values;
        }
    }
}
