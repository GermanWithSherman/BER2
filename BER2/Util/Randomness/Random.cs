using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BER2.Util.Randomness
{
    internal class Random
    {
        private static System.Random _random = new System.Random();

        internal static float Range(float min, float max)
        {
            return (float)_random.NextDouble() * (max-min)+min;
        }

        internal static int Range(int min, int max)
        {
            return _random.Next(min,max);
        }
    }

}