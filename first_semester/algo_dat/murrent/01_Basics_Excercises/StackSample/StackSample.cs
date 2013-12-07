// *******************************************************
// * <copyright file="StackSample.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("StackSample.Test")]

namespace StackSample
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The <see ref="StackSample"/> class and its interaction logic 
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class StackSample<T> : IStackSample<T>
    {
        /// <summary>
        /// The maximum stack items
        /// </summary>
        private int maxStackItems;

        /// <summary>
        /// The stack
        /// </summary>
        private readonly List<T> stack = new List<T>();

        /// <summary>
        /// Adds an object to the end of the stack
        /// </summary>
        /// <param name="stackItem">
        /// Type: T
        /// The object to add to the end of the stack
        /// </param>
        /// <exception cref="StackSample.StackOutOfSpaceException">
        /// </exception>
        public void Push(T stackItem)
        {
            if (this.StackCount < this.maxStackItems)
            {
                this.stack.Add(stackItem);
            }
            else
            {
                throw new StackOutOfSpaceException();
            }
        }

        /// <summary>
        /// Remove and return the object at the beginning of the concurrent stack
        /// </summary>
        /// <returns>Type: T
        /// The first item of the stack</returns>
        /// <exception cref="StackSample.StackIsEmptyException"></exception>
        public T Pop()
        {
            if (this.stack.Count > 0)
            {
                T stackItem = this.stack.Last();

                this.stack.Remove(stackItem);

                return stackItem;
            }

            throw new StackIsEmptyException();
        }

        /// <summary>
        /// Pick the first item out of the stack without deleting it
        /// </summary>
        /// <returns>The first item of the stack</returns>
        public T Peek()
        {
            return this.stack.Last();
        }

        /// <summary>
        /// Gets or sets the maximum stack items.
        /// </summary>
        /// <value>
        /// The maximum stack items.
        /// </value>
        public int MaxStackItems
        {
            get
            {
                return this.maxStackItems;
            }

            set
            {
                this.maxStackItems = value;
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the stack
        /// </summary>
        /// <value>
        /// The stack count.
        /// </value>
        internal int StackCount
        {
            get
            {
                return this.stack.Count;
            }
        }

        /// <summary>
        /// Gets the stack.
        /// </summary>
        /// <value>
        /// The stack.
        /// </value>
        internal IList<T> Stack
        {
            get
            {
                return this.stack;
            }
        }
    }
}