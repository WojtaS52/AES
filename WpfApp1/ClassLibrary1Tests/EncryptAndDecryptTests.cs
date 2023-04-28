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
    public class EncryptAndDecryptTests
        {
        [TestMethod()]
        public void EncryptTest()
        {


            byte[] key =
            {
                0x2b, 0x7e,0x15, 0x16,
                0x28,0xae,0xd2,0xa6,
                0xab,0xf7,0x15,0x88,
                0x09,0xcf,0x4f,0x3c,
            };

            byte[] key2 =
           {
                0x2b,0x28,0xab,0x09,
                0x7e,0xae, 0xf7,0xcf,
                0x15, 0xd2,0x15, 0x88,
                0x16,0xa6, 0x88, 0x3c,
            };


            Aes log1 = new Aes();
            keysGenerator log2 = new keysGenerator();

            byte[][] KeyMatrix = log2.KeyExpansion(key);

            EncryptAndDecrypt log3 = new EncryptAndDecrypt(KeyMatrix);

            
            byte[] tabBefore = new byte[] { 0x00, 0x00, 0x01, 0x01,
                                            0x03, 0x03, 0x07, 0x07, 
                                            0x0f, 0x0f, 0x1f, 0x1f, 
                                            0x3f, 0x3f, 0x7f, 0x7f };
             
            byte[] tabBefore2 = new byte[] { 0x00, 0x03, 0x0f, 0x3f,
                                            0x00, 0x03, 0x0f, 0x3f,
                                            0x01, 0x07, 0x1f, 0x7f,
                                            0x01, 0x07, 0x1f, 0x7f };
            
            
            byte[] tabAfter = new byte[] { 0x52, 0x02, 0xe6, 0x94, 0xec, 0xa2, 0xe2, 0x74, 0x71, 0x08, 0x86, 0xf1, 0xfd, 0xe3, 0xce, 0x82 };
            byte[] tab3 = log3.Encrypt(tabBefore,128);


            for(int i =0; i < tabAfter.Length; i++)
            {
                if (tab3[i] != tabAfter[i])
                {
                    Assert.Fail();
                }
            }
        Assert.IsTrue(true);
        }
        



        [TestMethod()]
        public void GetDataFromBlockTest()
        {
            Aes log = new Aes();
            byte[] data = new byte[32]
            {
                5, 6, 7, 8,
                5, 6, 7, 8,
                5, 6, 7, 8,
                5, 6, 7, 8,
                1, 2, 3, 4,
                1, 2, 3, 4,
                1, 2, 3, 4,
                1, 2, 3, 4,};

            byte[] bytes = {
                1, 2, 3, 4,
                1, 2, 3, 4,
                1, 2, 3, 4,
                1, 2, 3, 4,};

            byte[] bytes1 = log.getDataFromBlock(data, 1);

            for (int i = 0; i < 16; i++)
            {
                if (bytes[i] != bytes1[i])
                {
                    Assert.Fail();
                }
            }
            Assert.IsTrue(true);
        }


        [TestMethod()]
        public void GetKeyFromBlockTest()
        {
            EncryptAndDecrypt log = new EncryptAndDecrypt(null);
            Aes log2 = new Aes();

            byte[][] b = new byte[8][]
            {
                new byte[]  { 1, 2, 3, 4},
                new byte[]  { 1, 2, 3, 4},
                new byte[]  { 1, 2, 3, 4},
                new byte[]  { 1, 2, 3, 4},
                new byte[]  { 5, 6, 7, 8},
                new byte[]  { 5, 6, 7, 8},
                new byte[]  { 5, 6, 7, 8},
                new byte[]  { 5, 6, 7, 8}};

            byte[] bytes = new byte[16]
            {
                5, 6, 7, 8,
                5, 6, 7, 8,
                5, 6, 7, 8,
                5, 6, 7, 8};
            byte[] bytes1 = log2.getKeyFromBlock(b, 1);

            for(int i= 0; i < 16; i++)
            {
                if(bytes[i] != bytes1[i])
                {
                    Assert.Fail();
                }
            }
            Assert.IsTrue(true);

        }
    }
}