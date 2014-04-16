using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faktorielle;
using NUnit.Framework;

namespace RecursionUnitTest
{
    [TestFixture]
    public class UnitTest
    {
        private RecursionClass recursionClass;

        [SetUp]
        public void Init()
        {
            recursionClass = new RecursionClass();
        }

        [TestCase]
        public void TestLogger()
        {
            double result1 = recursionClass.FacultyRecursionStandard(150);
            double result2 = recursionClass.FacultyLoop(150);
            double result3 = recursionClass.FacultyRecursionTail(150, 1);
        }
    }
}
