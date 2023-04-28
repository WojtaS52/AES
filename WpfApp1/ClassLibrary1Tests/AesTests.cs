using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;

namespace ClassLibrary1.Tests
{
    [TestClass()]
    public class AesTests
    {
        [TestMethod()]
        public void RotateLeftTest()
        {

            Aes log = new Aes();

            byte[] tab1 = new byte[] { 1, 2, 3, 4 };
            byte[] tab2 = new byte[] { 2, 3, 4, 1 };
            byte[] tab3;



            tab3 = log.RotateLeft(tab1);

            int test = 0;
            for (int i = 0; i < 4; i++)
            {
                if (tab2[i] == tab3[i])
                {
                    test++;
                }

            }

            Assert.AreEqual(4, test);

        }

        [TestMethod()]
        public void RotateRightTest()
        {

            Aes log = new Aes();

            byte[] tab1 = new byte[] { 1, 2, 3, 4 };
            byte[] tab2 = new byte[] { 4, 1, 2, 3 };
            byte[] tab3;

            tab3 = log.RotateRight(tab1);

            int test = 0;
            for (int i = 0; i < 4; i++)
            {
                if (tab2[i] == tab3[i])
                {
                    test++;
                }

            }

            Assert.AreEqual(4, test);


        }


        

        [TestMethod()]
        public void MixColumnTest()
        {
            Aes log = new Aes();

            byte[] tabBeforeMix = new byte[] { 0x49, 0xdb, 0x87, 0x3b,
                                               0x45, 0x39, 0x53, 0x89,
                                               0x7f, 0x02, 0xd2, 0xf1,
                                               0x77, 0xde, 0x96, 0x1a };

            byte[] mixColumnEffect = log.MixColumns(tabBeforeMix);


            byte[] tabAfterMix = new byte[] { 0x58, 0x4d, 0xca, 0xf1,
                                              0x1b, 0x4b, 0x5a, 0xac,
                                              0xdb, 0xe7, 0xca, 0xa8,
                                              0x1b, 0x6b, 0xb0, 0xe5 };



            for (int i = 0; i < 16; i++)
            {
                if (mixColumnEffect[i] != tabAfterMix[i])
                {
                    Assert.Fail();
                }
            }
            Assert.IsTrue(true);
        }
            
           [TestMethod()]
           public void InvMixColumnTest()
           {
               Aes log = new Aes();

               byte[] tabBeforeMix = new byte[] { 0x58, 0x4d, 0xca, 0xf1,
                                                 0x1b, 0x4b, 0x5a, 0xac,
                                                 0xdb, 0xe7, 0xca, 0xa8,
                                                 0x1b, 0x6b, 0xb0, 0xe5 };

               byte[] invMixColums = log.InvMixColumns(tabBeforeMix);

               byte[] tabAfterMix = new byte[] { 0x49, 0xdb, 0x87, 0x3b,
                                               0x45, 0x39, 0x53, 0x89,
                                               0x7f, 0x02, 0xd2, 0xf1,
                                               0x77, 0xde, 0x96, 0x1a };

            for (int i = 0; i < 16; i++)
               {
                   if (invMixColums[i] != tabAfterMix[i])
                   {
                       Assert.Fail();
                   }
               }
               Assert.IsTrue(true);
           }

        [TestMethod()]
        public void MoveRowsToMatrixTest()
        {
            Aes log = new Aes();
            byte[] r1 = { 1, 2, 3, 4 };
            byte[] r2 = { 5, 6, 7, 8 };
            byte[] r3 = { 9, 10, 11, 12 };
            byte[] r4 = { 13, 14, 15, 16 };

            byte[] checkTab =
            {
            1, 5, 9, 13,
            2, 6, 10, 14,
            3, 7, 11, 15,
            4, 8, 12, 16};

            byte[] newTab = new byte[16];
            newTab = log.moveRowstoMatrixforTest(r1, r2, r3, r4);

            for (int i = 0; i < 16; i++)
            {
                if (checkTab[i] != newTab[i])
                {
                    Assert.Fail();
                }
            }
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void inverseShiftRowsTest()
        {
            Aes log = new Aes();


            byte[] tab1 =
            {
                1,6,11,16,
                5,10,15,4,
                9,14,3,8,
                13,2,7,12
            };

            byte[] bytes = log.inverseShiftRows(tab1);

            byte[] tab2 =
            {
            1, 2, 3, 4,
            5, 6, 7, 8,
            9, 10, 11, 12,
            13, 14, 15, 16};


            for (int i = 0; i < 16; i++)
            {
                if (tab2[i] != bytes[i])
                {
                    Assert.Fail();
                }

                Assert.IsTrue(true);

            }
        }

        [TestMethod()]
        public void ShitftRowsTest()
        {
            Aes log = new Aes();
            byte[] tab =
            {
            1, 2, 3, 4,
            5, 6, 7, 8,
            9, 10, 11, 12,
            13, 14, 15, 16};

            byte[] bytes = log.ShiftRows(tab);

            byte[] afterShiftRowsTabForTestsCheck =
            {
                1,6,11,16,
                5,10,15,4,
                9,14,3,8,
                13,2,7,12
            };
            for (int i = 0; i < 16; i++)
            {
                if (afterShiftRowsTabForTestsCheck[i] != bytes[i])
                {
                    Assert.Fail();
                }
            }
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void GetBoxTest()
        {
            byte[] bytes = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,
                            17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };

            Aes log = new Aes();

            byte[] bytes1 = log.getDataFromBlock(bytes, 1);

            byte[] bytes2 = { 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };

            for (int i = 0; i < 16; i++)
            {
                if (bytes2[i] != bytes1[i])
                {
                    Assert.Fail();
                }
            }
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void zeroPaddingTest()
        {
            byte[] bytes = { 1, 2, 3, 4, 5 };
            byte[] bytes3 = new byte[16];

            for (int i = 0; i < 5; i++)
            {
                bytes3[i] = bytes[i];
            }
            Aes log = new Aes();

            byte[] bytes1 = log.zeroPadding(bytes3);

            byte[] bytes2 = { 1, 2, 3, 4, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5 };

            for (int i = 0; i < 16; i++)
            {
                if (bytes2[i] != bytes1[i])
                {
                    Assert.Fail();
                }
            }
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void removeAddedZerosTest()
        {

            byte[] bytes2 = { 1, 2, 3, 4, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5 };
            byte[] bytes = { 1, 2, 3, 4, 5 };
            byte[] bytes3 = new byte[16];

            for (int i = 0; i < 5; i++)
            {
                bytes3[i] = bytes[i];
            }
            Aes log = new Aes();

            byte[] bytes1 = log.removeAddedZeros(bytes2);



            for (int i = 0; i < 16; i++)
            {
                if (bytes3[i] != bytes1[i])
                {
                    Assert.Fail();
                }
            }
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void SubBytesTests()
        {
            Aes log = new Aes();

            byte[] bytes =
                { 0xa4, 0x68, 0x6b, 0x02,
                0x9c, 0x9f, 0x5b, 0x6a,
                0x7f, 0x35, 0xea, 0x50,
                0xf2, 0x2b, 0x43, 0x49};
            byte[] bytes2 =
            {
                0x49, 0x45, 0x7f, 0x77,
                0xde, 0xdb, 0x39, 0x02,
                0xd2, 0x96, 0x87, 0x53,
                0x89, 0xf1, 0x1a, 0x3b };

            byte[] bytes3 = log.SubBytes(bytes);

            for (int i = 0; i < 16; i++)
            {
                if (bytes3[i] != bytes2[i])
                {
                    Assert.Fail();
                }
            }
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void RotWordTest()
        {
            Aes log = new Aes();

            byte[] r1 = { 1, 2, 3, 4 };
            byte[] r2 = { 2, 3, 4, 1 };
            byte[] r3 = log.RotateWord(r1);

            for (int i = 0; i < 4; i++)
            {
                if (r2[i] != r3[i])
                {
                    Assert.Fail();
                }
            }
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void SubWordTest()
        {
            Aes log = new Aes();

            byte[] bytes =
                { 0xa4, 0x68, 0x6b, 0x02 };

            byte[] bytes2 =
                { 0x49, 0x45, 0x7f, 0x77 };

            byte[] bytes3 = log.SubWord(bytes);

            for (int i = 0; i < 4; i++)
            {
                if (bytes3[i] != bytes2[i])
                {
                    Assert.Fail();
                }
            }
            Assert.IsTrue(true);

        }
    }
}