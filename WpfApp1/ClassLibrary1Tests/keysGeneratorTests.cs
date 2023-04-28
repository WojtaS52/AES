using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ClassLibrary1.Tests
{
    [TestClass()]
    public class keysGeneratorTests
    {
        [TestMethod()]
        public void keyGenerateTest()
        {
            string elo;

            elo = keysGenerator.keyGenerate(25);

            Assert.AreEqual(elo.Length, 25);

        }

        

        [TestMethod()]
        public void KeyExpansionTest() {

            
            keysGenerator kg = new keysGenerator();
            
            byte[] key =
            {
                0x2b, 0x7e,0x15, 0x16,
                0x28,0xae,0xd2,0xa6,
                0xab,0xf7,0x15,0x88,
                0x09,0xcf,0x4f,0x3c, 
            };

          


            byte[][] tab = new byte[11][];

            tab = kg.KeyExpansion(key);

            byte[][] tab2 = new byte[3][]
            {
                new byte[] { 0x2b , 0x7e, 0x15, 0x16},
                new byte[] { 0x7a, 0x96, 0xb9, 0x43},
                new byte[] { 0xb6, 0x63, 0x0c, 0xa6},

            };

            for(int i =0; i <4; i++)
            {   
                if (tab[0][i] != tab2[0][i])
                {
                    Assert.Fail();
                }

                if (tab[9][i] != tab2[1][i])
                {
                    Assert.Fail();
                }


                if (tab[43][i] != tab2[2][i])
                {
                    Assert.Fail();
                }


            }
            Assert.IsTrue(true);


        }

        

    }
}