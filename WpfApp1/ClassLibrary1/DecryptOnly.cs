using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class DecryptOnly
    {
        private byte[][] key;

        public DecryptOnly(byte[][] key)
        {
            this.key = key;
        }

        private Aes aes = new Aes();

       
        public byte[] Decrypt(byte[] encryptedData, int keySize)
        {


            int rounds = 0;
            if (keySize == 256)
            {
                rounds = 13;
            }
            else if (keySize == 192)
            {
                rounds = 11;
            }
            else if (keySize == 128)
            {
                rounds = 9;
            }

            byte[] bytes = new byte[encryptedData.Length];

            for (int i = 0; i < (encryptedData.Length/16); i++)
            {
                byte[] block = aes.getDataFromBlock(encryptedData, i);
                block = DecryptStart(block, aes.getKeyFromBlock(key, rounds+1), keySize);

                for (int j = rounds-1; j >= 0; j--)
                {
                    block = DecryptMain(block, key, j);
                    
                }
                block = DecryptEnd(block, key);

                for (int o = 0; o < 16; o++)
                {
                    bytes[16 * i + o] = block[o];
                }

                
            }
            bytes = aes.removeAddedZeros(bytes);
            return bytes;
        }

        public byte[] DecryptStart(byte[] data, byte[] key, int keySize)
        {
            byte[] bytes = new byte[16];
            /*
            int rounds = 0;

            if (keySize == 128)
            {
                rounds = 10;
            }
            else if (keySize == 192)
            {
                rounds = 12;
            }
            else if (keySize == 256)
            {
                rounds = 14;
            }*/

            bytes = aes.AddRoundKey(data, key);
            bytes = aes.inverseShiftRows(bytes);
            bytes = aes.invSubBytes(bytes);

            return bytes;
        }


        private byte[] DecryptMain(byte[] data, byte[][] key, int it)
        {
            byte[] new_array = aes.AddRoundKey(data, aes.getKeyFromBlock(key, it+1));
            new_array = aes.InvMixColumns(new_array);
            new_array = aes.inverseShiftRows(new_array);
            new_array = aes.invSubBytes(new_array);

            return new_array;
        }




        private byte[] DecryptEnd(byte[] data, byte[][] key)
        {
            byte[] new_array = aes.AddRoundKey(data, aes.getKeyFromBlock(key,0));
            return new_array;
        }
    }

}





