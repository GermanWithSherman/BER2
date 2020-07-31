using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BER2.Language
{
    public static class Language
    {
        public static string Skimpiness(int skimpiness)
        {
            switch (skimpiness)
            {
                case int n when n >= 100:
                    return "Extremely slutty";
                case int n when n >= 90:
                    return "Whorish";
                case int n when n >= 80:
                    return "Very slutty";
                case int n when n >= 70:
                    return "Slutty";
                case int n when n >= 60:
                    return "Sexy";
                case int n when n >= 50:
                    return "Attractive";
                case int n when n >= 40:
                    return "Default";
                case int n when n >= 30:
                    return "Conservative";
                case int n when n >= 20:
                    return "Prudish";
                case int n when n >= 10:
                    return "Very Prudish";
                default:
                    return "Not skimpy";
            }
        }

    }
}
