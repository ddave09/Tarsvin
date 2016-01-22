using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tarsvin.Runner;
using Tarsvin.Runner.Report;

namespace RunnerHelper.Test
{
    [TestClass]
    public class NamespaceGroupingTest
    {
        ICollection<IndividualTestState> tests;

        [TestMethod]
        public void Can_Count_Unique_Namespaces()
        {
            this.tests = new List<IndividualTestState>();

            IndividualTestState test = new IndividualTestState();
            test.NameSpace = "A.B.C.a";
            test.TestName = "a";
            tests.Add(test);

            test = new IndividualTestState();
            test.NameSpace = "A.B.C.b";
            test.TestName = "b";
            tests.Add(test);

            test = new IndividualTestState();
            test.NameSpace = "A.B.c";
            test.TestName = "c";
            tests.Add(test);

            test = new IndividualTestState();
            test.NameSpace = "A.B.a";
            test.TestName = "a";
            tests.Add(test);

            test = new IndividualTestState();
            test.NameSpace = "A.B.C.D.e";
            test.TestName = "e";
            tests.Add(test);

            var grouped = tests.GroupBy(x => new { x.NameSpace });

            Assert.AreEqual(5, grouped.Count());
        }

        [TestMethod]
        public void Can_Group_Duplicate_Namespaces()
        {
            this.tests = new List<IndividualTestState>();

            IndividualTestState test = new IndividualTestState();
            test.NameSpace = "A.B.C";
            test.TestName = "a";
            tests.Add(test);

            test = new IndividualTestState();
            test.NameSpace = "A.B.C";
            test.TestName = "b";
            tests.Add(test);

            test = new IndividualTestState();
            test.NameSpace = "A.B";
            test.TestName = "c";
            tests.Add(test);

            test = new IndividualTestState();
            test.NameSpace = "A.B";
            test.TestName = "a";
            tests.Add(test);

            test = new IndividualTestState();
            test.NameSpace = "A.B.C.D";
            test.TestName = "e";
            tests.Add(test);

            var grouped = tests.GroupBy(x => new { x.NameSpace });

            Assert.AreEqual(3, grouped.Count());
        }


    }
}
