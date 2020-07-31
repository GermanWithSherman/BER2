using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BER2.Extensions
{
    public static class LongExtensions
    {

        public static string ToStringMoney(this long money)
        {
            double m = money / 100d;
            return m.ToString("C", CultureInfo.CreateSpecificCulture("de"));
        }
    }
}
