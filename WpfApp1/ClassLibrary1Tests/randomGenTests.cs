using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Tests
{
    [TestClass()]
    public class randomGenTests
    {
        [TestMethod()]
        public void generateRandomIntTest()
        {
            randomGen logic = new randomGen();

            int test;

            test = logic.generateRandomInt(1, 1);
            Assert.AreEqual(1, test);

        }

    }
}