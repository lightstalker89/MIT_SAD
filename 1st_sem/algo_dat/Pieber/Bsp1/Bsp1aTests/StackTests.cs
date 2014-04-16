using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bsp1a;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Bsp1a.Tests
{
    [TestClass]
    public class StackTest
    {
        [TestMethod]
        public void ctorTest()
        {
            Stack<double> s = new Stack<double>();

            PrivateObject poS = new PrivateObject(s);

            Assert.IsTrue(((int)poS.GetField("lastIndex")) < 0);
            Assert.AreEqual(0, ((double[])poS.GetField("stack")).Length);
        }

        [TestMethod]
        public void PushTest()
        {
            Stack<double> s = new Stack<double>();

            s.Push(23.3d);
            s.Push(8437209.12d);
            s.Push(7437209.12d);
            s.Push(6437209.12d);
            s.Push(5437209.12d);
            s.Push(4437209.12d);
            s.Push(3437209.12d);
            s.Push(2437209.12d);
            s.Push(1437209.12d);

            PrivateObject poS = new PrivateObject(s);

            double[] privateStack = (double[])poS.GetField("stack");

            Assert.AreEqual(8, ((int)poS.GetField("lastIndex")));
            Assert.IsTrue(privateStack.Length >= 9);
            Assert.AreEqual(23.3d, privateStack[0]);
            Assert.AreEqual(8437209.12d, privateStack[1]);
            Assert.AreEqual(7437209.12d, privateStack[2]);
            Assert.AreEqual(6437209.12d, privateStack[3]);
            Assert.AreEqual(5437209.12d, privateStack[4]);
            Assert.AreEqual(4437209.12d, privateStack[5]);
            Assert.AreEqual(3437209.12d, privateStack[6]);
            Assert.AreEqual(2437209.12d, privateStack[7]);
            Assert.AreEqual(1437209.12d, privateStack[8]);
        }

        [TestMethod]
        [ExpectedException(typeof(StackEmptyException))]
        public void PopTest()
        {
            Stack<double> s = new Stack<double>();

            s.Push(23.3d);
            s.Push(8437209.12d);
            s.Push(7437209.12d);
            Assert.AreEqual(7437209.12d, s.Pop());
            s.Push(6437209.12d);
            s.Push(5437209.12d);
            s.Push(4437209.12d);
            s.Push(3437209.12d);
            s.Push(2437209.12d);
            Assert.AreEqual(2437209.12d, s.Pop());
            s.Push(1437209.12d);
            Assert.AreEqual(1437209.12d, s.Pop());
            Assert.AreEqual(3437209.12d, s.Pop());
            Assert.AreEqual(4437209.12d, s.Pop());
            Assert.AreEqual(5437209.12d, s.Pop());
            Assert.AreEqual(6437209.12d, s.Pop());
            Assert.AreEqual(8437209.12d, s.Pop());
            Assert.AreEqual(23.3d, s.Pop());

            PrivateObject poS = new PrivateObject(s);

            double[] privateStack = (double[])poS.GetField("stack");

            Assert.IsTrue(((int)poS.GetField("lastIndex")) < 0);
            Assert.IsTrue(privateStack.Length >= 7);
            Assert.AreEqual(default(double), privateStack[0]);
            Assert.AreEqual(default(double), privateStack[1]);
            Assert.AreEqual(default(double), privateStack[2]);
            Assert.AreEqual(default(double), privateStack[3]);
            Assert.AreEqual(default(double), privateStack[4]);
            Assert.AreEqual(default(double), privateStack[5]);
            Assert.AreEqual(default(double), privateStack[6]);
            s.Pop();
        }

        [TestMethod]
        public void PeekTest()
        {
            Stack<double> s = new Stack<double>();

            s.Push(23.3d);
            s.Push(8437209.12d);
            Assert.AreEqual(8437209.12d, s.Peek());
            s.Push(7437209.12d);
            Assert.AreEqual(7437209.12d, s.Peek());
            s.Push(6437209.12d);
            s.Push(5437209.12d);
            Assert.AreEqual(5437209.12d, s.Peek());
            s.Push(4437209.12d);
            s.Push(3437209.12d);
            s.Push(2437209.12d);
            s.Push(1437209.12d);
            Assert.AreEqual(1437209.12d, s.Peek());
        }
    }
}
