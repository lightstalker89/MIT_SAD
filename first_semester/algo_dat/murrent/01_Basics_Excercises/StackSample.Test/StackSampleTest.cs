// *******************************************************
// * <copyright file="StackSampleTest.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace StackSample.Test
{
    using System.Globalization;
    using System.Linq;

    using NSubstitute;

    using NUnit.Framework;

    /// <summary>
    /// The <see ref="StackSampleTest"/> class and its interaction logic 
    /// </summary>
    [TestFixture]
    public class StackSampleTest
    {
        /// <summary>
        /// </summary>
        private StackSample<string> stack;

        /// <summary>
        /// </summary>
        [SetUp]
        public void Init()
        {
            stack = Substitute.For<StackSample<string>>();
            stack.MaxStackItems = 10;
        }

        /// <summary>
        /// Test properties of the class
        /// </summary>
        [TestCase]
        public void StackSampleClassTest()
        {
            Assert.That(stack.MaxStackItems.Equals(10));
        }

        /// <summary>
        /// Add max allowed items to the stack
        /// </summary>
        [TestCase]
        public void PushToMaxItemCountTest()
        {
            for (int i = 0; i < 10; ++i)
            {
                this.stack.Push("Item " + i.ToString(CultureInfo.InvariantCulture));
            }

            Assert.That(this.stack.StackCount.Equals(10));
        }

        /// <summary>
        /// Add more items to the stack than allowed
        /// </summary>
        [TestCase]
        public void PushOverMaxItemCountTest()
        {
            Assert.Throws<StackOutOfSpaceException>(
                () =>
                    {
                        for (int i = 0; i <= 12; ++i)
                        {
                            this.stack.Push("Item " + i.ToString(CultureInfo.InvariantCulture));
                        }
                    });

            Assert.That(this.stack.StackCount.Equals(10));
        }

        /// <summary>
        /// Pop items from the stack with no items
        /// </summary>
        [TestCase]
        public void PopEmptyStackTest()
        {
            EmptyStack();

            Assert.Throws<StackIsEmptyException>(() => this.stack.Pop());
        }

        /// <summary>
        /// Pop iitems from the stack filled with items
        /// </summary>
        [TestCase]
        public void PopTest()
        {
            FillStack();

            for (int i = 9; i >= 0; --i)
            {
                this.stack.Pop();
            }

            Assert.That(this.stack.StackCount.Equals(0));
        }

        /// <summary>
        /// Get an item without deleting from the stack
        /// </summary>
        [TestCase]
        public void PeekTest()
        {
            this.EmptyStack();
            this.FillStack();

            int countBefore = this.stack.StackCount;

            string item = this.stack.Peek();

            int countAfter = this.stack.StackCount;

            Assert.That(countBefore.Equals(countAfter));
            Assert.NotNull(item);
            Assert.Contains(item, this.stack.Stack.ToList());
        }

        /// <summary>
        /// Fills the stack.
        /// </summary>
        private void FillStack()
        {
            for (int i = this.stack.StackCount; i < this.stack.MaxStackItems; ++i)
            {
                this.stack.Push("Item " + i);
            }
        }

        /// <summary>
        /// Empties the stack.
        /// </summary>
        private void EmptyStack()
        {
            if (this.stack.StackCount > 0)
            {
                for (int i = this.stack.StackCount; i >= 0; --i)
                {
                    this.stack.Pop();
                }
            }
        }
    }
}