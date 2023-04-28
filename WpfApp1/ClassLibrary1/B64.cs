using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{

    public class B64
    {
        private Dictionary<string, string> base64 = new Dictionary<string, string>();


        public string GetBits(byte[] bytes)
        {
            

            StringBuilder log = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                for (int j = 7; j >= 0; j--)
                {
                    log.Append((bytes[i] >> j) & 1);
                }
            }
            return log.ToString();


        }

        public void Base64()
        {
            int offset = 65; 
            for (int i = 0; i < 62; i++)
            {
                if (i == 26)
                {
                    offset = 71; 
                }
                if (i == 52)
                {
                    offset = -4; 
                }
                StringBuilder binaryString = new StringBuilder(Convert.ToString(i, 2));
                for (int j = binaryString.Length; j < 6; j++)
                {
                    binaryString.Insert(0, "0");
                }
                base64[binaryString.ToString()] = Convert.ToString((char)(i + offset));
            }
            base64["111110"] = "+";
            base64["111111"] = "/";
        }



        public int bitsToNumber(string bits)
        {
            int result = 0;
            string reverse = new string(bits.Reverse().ToArray());

            for (int i = 0; i < 8; i++)
            {
                string tmp = reverse.Substring(i, 1);
                if (tmp == "1")
                {
                    int temp = 1;
                    for (int j = 0; j < i; j++)
                    {
                        temp *= 2;
                    }
                    result += temp;
                }
            }
            return result;
        }

        

    }
}
