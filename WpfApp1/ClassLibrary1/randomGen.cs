using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class randomGen
    {

        public int generateRandomInt(int min, int max)
        {
            return this.GetHashCode() % (max - min + 1) + min;
        }

        public char generateRandomCharapter()
        {
            return (char)generateRandomInt(33, 126);
        }


    }
}
