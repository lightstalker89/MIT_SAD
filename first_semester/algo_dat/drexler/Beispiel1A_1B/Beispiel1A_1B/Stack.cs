//-----------------------------------------------------------------------
// <copyright file="Stack.cs" company="MD Development">
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
    /// My stack implementation
    /// </summary>
    /// <typeparam name="T">type of the items</typeparam>
    public class Stack<T>
    {
        private IList<T> collection;
        private int stackSize;

        public Stack(int size)
        {
            this.stackSize = size;
            this.collection = new List<T>();
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
                if (collection.Count < this.stackSize)
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

        /// <summary>
        /// Representing the items within the stack as a string
        /// </summary>
        /// <returns>Stack items as a string</returns>
        public string MyToString()
        {
            string values = string.Empty;

            if (this.collection != null && this.collection.Count > 0)
            {
                do
                {
                    values += this.Pop().ToString() + " ";
                } while (this.collection.Count != 0);
            }

            return values;
        }
    }
}
