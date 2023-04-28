using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class EncryptAndDecrypt
    {
        private byte[][] key;
        
        public EncryptAndDecrypt(byte[][] key)
        {
            this.key = key;
        }



        private Aes aes = new Aes();


        public byte[] Encrypt(byte[] data, int keySize)
        {

            data = aes.zeroPadding(data);
            byte[] result = new byte[data.Length];

            for (int i = 0; i < data.Length / 16; i++)
            {
                byte[] block = aes.getDataFromBlock(data, i);
                block = EncryptStart(block, aes.getKeyFromBlock(key, 0));
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
               
                else
                {
                    throw InvalidDataException();
                }

                for (int j = 1; j <= rounds; j++)
                {
                    block = EncryptMain(block, aes.getKeyFromBlock(key, j), j);
                }
                byte[] bytes = EncryptEnd(block, aes.getKeyFromBlock(key, rounds + 1), keySize);
                

                for (int o = 0; o < 16; o++)
                {
                    result[16 * i + o] = bytes[o];
                }

            }
            return result;
        }

        private Exception InvalidDataException()
        {
            throw new FieldAccessException();
        }

        private byte[] EncryptStart(byte[] data, byte[] key)
        {
            byte[] new_array = aes.AddRoundKey(data, key);
            return new_array;
        }

        private byte[] EncryptMain(byte[] data, byte[] key, int it)
        {
            byte[] bytes = new byte[16];


            //w tej rundzie środkowej że tak nazwe mamy 4 operacje zgodnie z pdfem o aes'ie
            bytes = aes.SubBytes(data);
            bytes = aes.ShiftRows(bytes);
            bytes = aes.MixColumns(bytes);
            bytes = aes.AddRoundKey(bytes, key);

            return bytes;
        }

        private byte[] EncryptEnd(byte[] data, byte[] key, int keySize)
        {
            byte[] bytes = new byte[16];

            int rounds = 0; //do naszego AddRoundkeya potrzebujemy, bo on ma jako argument to cos

            if (keySize == 128)
            {
                rounds = 10;
            }
            else if (keySize == 256)
            {

                rounds = 14;
            }
            else if (keySize == 192)
            {
                rounds = 12;

            }

            //here in this final round we have 3 operations, no mixColumn

            bytes = aes.SubBytes(data);
            bytes = aes.ShiftRows(bytes);
            bytes = aes.AddRoundKey(bytes, key);

            return bytes;

        }




    }
}
