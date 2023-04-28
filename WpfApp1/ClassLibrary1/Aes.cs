using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClassLibrary1
{
    public class Aes
    {
        private static byte[] to_mix =
            {0x02, 0x03, 0x01, 0x01,
            0x01, 0x02, 0x03, 0x01,
            0x01, 0x01, 0x02, 0x03,
            0x03, 0x01, 0x01, 0x02};

        private static byte[] inv_to_mix =
        {
            0x0e, 0x0b, 0x0d, 0x09,
            0x09, 0x0e, 0x0b, 0x0d,
            0x0d, 0x09, 0x0e, 0x0b,
            0x0b, 0x0d, 0x09, 0x0e
        };
        //a'la rcom
        private static int[] pomocnicza =
        {
            0, 4, 8, 12, 1, 5, 9, 13, 2, 6, 10, 14, 3, 7, 11, 15
        };

        //wg dokumentacji jakiegoś amerykanina jest git
        //this method takes two byte arrays as parameters
        //in our code tab represents the current state of cipher
        // klucz represents the key schedule for the current method
        public byte[] AddRoundKey(byte[] tab, byte[] klucz)//jak nie działa wez klucz get'em/ale jak :>?
        {
            byte[] wynik = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                wynik[i] = (byte)(tab[i] ^ klucz[i]);
            }
            return wynik;
        }

        private int removeMinusInByte(int b)
        {

            if (b < 0)
            {
                b = b + 256;
            }

            return b;
        }

        public byte[] ShiftRows(byte[] tab) 
        {

            byte[] r1 = { tab[0], tab[4], tab[8], tab[12] };
            byte[] r2 = { tab[1], tab[5], tab[9], tab[13] };
            byte[] r3 = { tab[2], tab[6], tab[10], tab[14] };
            byte[] r4 = { tab[3], tab[7], tab[11], tab[15] };

            //1 rzad nie ruszamy

            //2 rzad raz w lewo
            r2 = RotateLeft(r2);

            //3 rzad 2 razy w lewo
            r3 = RotateLeft(r3);
            r3 = RotateLeft(r3);

            //4 rzad 3 razy w lewo
            r4 = RotateLeft(r4);
            r4 = RotateLeft(r4);
            r4 = RotateLeft(r4);

            return moveRowsToMatrix(r1,r2,r3,r4);
        }

        public byte[] RotateLeft(byte[] row)
        {
            byte[] rotated = { row[1], row[2], row[3], row[0] };
            return rotated;
        }

        public byte[] RotateRight(byte[] row)
        {
            byte[] rotated = { row[3], row[0], row[1], row[2] };
            return rotated;
        }

        //https://www.youtube.com/watch?v=Tx_37dF03ig - helpful video
        
        public byte[] MixColumns(byte[] tab)
        {
            byte[] new_tab = new byte[16];

            for (int i = 0; i < 16; i++)
            {
                int row = i % 4;
                int column = i / 4;

                byte[] temp = new byte[4];

                for (int j = 0; j < 4; j++)
                {
                    int dec = tab[column *4 + j];
                    int mul = to_mix[row * 4 + j];

                    if (mul == 0x03)
                    {
                        mul = 0x02;
                    }
                    dec = removeMinusInByte(dec);
                    int value = dec * mul;
                    if (to_mix[row * 4 + j] == 2 && value > 255)
                    {
                        value = value ^ 0x1B;
                    }
                    if (to_mix[row * 4 + j] == 3)
                    {
                        value = value ^ dec;
                        if (value > 255)
                        {
                            value = value ^ 0x1B;
                        }
                    }
                    temp[j] = (byte)value;
                }
                new_tab[i] = xor_Result(temp);

            }
            return new_tab;
        }

        public byte xor_Result(byte[] value)
        {
            byte final_result = (byte)(value[0] ^ value[1] ^ value[2] ^ value[3]);
            return final_result;
        }

        private byte[] moveRowsToMatrix(byte[] r1, byte[] r2, byte[] r3, byte[] r4)
        {
            byte[] matrix = new byte[16];

            for (int i = 0; i < 4; i++)
            {
                matrix[i*4] = r1[i];
                matrix[i * 4+1] = r2[i];
                matrix[i*4 +2] = r3[i];
                matrix[i*4+3] = r4[i];
            }

            return matrix;
        }

        public byte[] moveRowstoMatrixforTest(byte[] r1, byte[] r2, byte[] r3, byte[] r4)
        {
            return moveRowsToMatrix(r1, r2, r3, r4);
        }

        public byte[] inverseShiftRows(byte[] tab)
        {
            byte[] r1 = { tab[0], tab[4], tab[8], tab[12] };
            byte[] r2 = { tab[1], tab[5], tab[9], tab[13] };
            byte[] r3 = { tab[2], tab[6], tab[10], tab[14] };
            byte[] r4 = { tab[3], tab[7], tab[11], tab[15] };

            r2 = RotateRight(r2);

            r3 = RotateRight(r3);
            r3 = RotateRight(r3);

            r4 = RotateRight(r4);
            r4 = RotateRight(r4);
            r4 = RotateRight(r4);


            return moveRowsToMatrix(r1, r2, r3, r4);
        }

        private int mulWithRemoveOverflow(int val)
        {
            val = val * 2;
            if (val > 255)
            {
                val = val ^ (0x1B);
                val = val - 256;
            }
            return val;
        }

        //method of invert our mix column
        public byte[] InvMixColumns(byte[] tab)
        {
            byte[] result = new byte[16];
           
            for(int i = 0; i < 16; i++)
            {
                int row = i % 4;
                int column = i / 4;

                byte[] temp = new byte[4];
                for (int j = 0; j < 4; j++)
                {
                    int dec = tab[column  * 4 + j];
                    int mul = inv_to_mix[row * 4 + j];

                    dec = removeMinusInByte(dec);
                    int value = dec;

                    switch(mul)
                    {
                        case 9:
                            for (int k = 0; k < 3; k++)
                            {
                                value = mulWithRemoveOverflow(value);
                            }
                            value = value ^ dec;
                            break;
                        case 11:
                            for(int k = 0; k < 2; k++)
                            {
                                value = mulWithRemoveOverflow(value);
                            }
                            value = value ^ dec;
                            value = mulWithRemoveOverflow(value);
                            value = value ^ dec;
                            break;
                        case 13:
                            value = mulWithRemoveOverflow(value);
                            value = value ^ dec;
                            for(int k = 0; k < 2; k++)
                            {
                                value = mulWithRemoveOverflow(value);
                            }
                            value = value ^ dec;
                            break;
                        case 14:
                            for(int k = 0;k < 2; k++)
                            {
                                value = mulWithRemoveOverflow(value);
                                value = value ^ dec;
                            }
                            value = mulWithRemoveOverflow(value);
                            break;

                    }
                    temp[j] = (byte)value;
                }
                result[i] = xor_Result(temp);
            }
            return result;
        }

        //dividing into blocks
        public byte[] getDataFromBlock(byte[] array, int digit )
        {
            byte[] new_tab = new byte[16];

            int i = 0;

            while(i < 16)
            {
                new_tab[i] = array[16 * digit + i];             
                i+=1;
            }

            return new_tab;
        }

        // Kod daje nam klucz 16 bajtowy, o zadanym digit -> to numer indeksu klucza, 
        public byte[] getKeyFromBlock(byte[][] ourKey, int digit)
        {
            int z = 0;
            byte[] result = new byte[16];
            for (int i = 0; i < 16; i+=4)
            {
                for(int j=0; j < 4; j++)
                {
                    result[i + j] = ourKey[4*digit+z][j];
                }
                z++;
            }
            return result;
            
        }


        
        // zero padding to 16 bytes 
        public byte[] zeroPadding(byte[] array)
        {
            int pom = (array.Length) % 16;
            int lastN = pom;

            if ( lastN == 0)
            {
                return array;
            }
            
            int temp = array.Length;
            int addedBytes = 16 - lastN;
            byte[] completedArray = new byte[addedBytes+temp];

            System.Array.Copy(array,0, completedArray, 0, temp);

            completedArray[temp] = (byte)0xFF; // 0xFF is maximum size of bytes

            for (int i = temp + 1; i < (addedBytes + array.Length); i++)
            {
                completedArray[i] = (byte)0x00;

            }               

            return completedArray;

            

        }
        // remove adding zeros
        // array to nasza ta tablica
        public byte[] removeAddedZeros(byte[] array)
        {
            int digit0 = 0;

            
            int len = array.Length - 1;
            int i = len;
            int condition = array.Length - 16;

            //idziemy jakby od przedostatniego bajtu bo na ostatnim jest ta liczba tych 0 
            while (i >= condition)
            {
                
                if (array[i] == 0)
                {
                    digit0 += 1; // inkremetnacja
                }
                else if(array[i] != 0)
                {
                    break;
                }
                i--; // deikrementacja

            }

            if (digit0 != 0 || array[len] == (byte)0xFF)
            {
                digit0 = digit0 + 1;
            }

            byte[] firstForm = new byte[len -digit0+1];


            for(int j =0; j<len-digit0+1; j++)
            {
                firstForm[j] = array[j];


            }

            return firstForm;
        }
        
        public byte[] SubBytes(byte[] tab)
        {

            for(int i = 0; i < 16; i++)
            {
                int intFromByte = tab[i];
                intFromByte = removeMinusInByte(intFromByte);
                tab[i] = (byte)SBox.getBox(intFromByte/16, intFromByte % 16);
            }
            
            return tab;
        }

        public byte[] SubWord(byte[] tab)
        {

            for (int i = 0; i < 4; i++)
            {
                int intFromByte = tab[i];
                intFromByte = removeMinusInByte(intFromByte);
                tab[i] = (byte)SBox.getBox(intFromByte / 16, intFromByte % 16);
            }

            return tab;
        }


        public byte[] invSubBytes(byte[] tab)
        {
            for (int i = 0; i < 16; i++)
            {
                int intFromByte = tab[i];
                intFromByte = removeMinusInByte(intFromByte);
                tab[i] = (byte)SBox.invGetBox(intFromByte / 16, intFromByte % 16);
            }

            return tab;


        }

        public byte[] RotateWord(byte[] row)
        {
            byte[] rotWord = RotateLeft(row);
            return rotWord;
        }


    }
}
