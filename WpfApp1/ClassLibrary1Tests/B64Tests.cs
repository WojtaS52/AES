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
    public class B64Tests
    {
        [TestMethod()]
        public void bitsToNumberTest()
        {
            B64 log = new B64();

            string n1 = "00001111";
            int n2 = 15;
            int n3;
            n3 = log.bitsToNumber(n1);

            if(n3 == n2)
            {
                Assert.IsTrue(true);
            }


        }

        [TestMethod()]
        public void GetBitsTest()
        {
            B64 log = new B64();
            byte[] bytes2 = new byte[] { 2, 4 };
            byte[] bytes = new byte[] { 51 };
            string bits = "0000001000000100";
            Assert.AreEqual(bits, log.GetBits(bytes2));
        }
    }
}