using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BER2.Util.Math
{
    internal class Mathb
    {
        internal static float Pow(float b, int power)
        {
            return (float)System.Math.Pow(b, (double)power);
        }

        internal static int RoundToInt(float v)
        {
            return (int)System.Math.Round(v);
        }

        internal static int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }

        internal static int CeilToInt(float v)
        {
            return (int)System.Math.Ceiling(v);
        }

        internal static int Max(int x, int y)
        {
            if (x >= y) return x;
            return y;
        }
    }
}
