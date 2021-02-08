using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Music.SDK.Extensions
{
    public static partial class BigIntegerExtensions
    {
        // this have to be used for extend BigInteger
        public static String ToRadixString(this BigInteger value, int radix)
        {
            if (radix <= 1 || radix > 36)
                throw new ArgumentOutOfRangeException(nameof(radix));
            if (value == 0)
                return "0";

            bool negative = value < 0;

            if (negative)
                value = -value;

            StringBuilder sb = new StringBuilder();

            for (; value > 0; value /= radix)
            {
                int d = (int)(value % radix);

                sb.Append((char)(d < 10 ? '0' + d : 'A' - 10 + d));
            }

            return (negative ? "-" : "") + string.Concat(sb.ToString().Reverse());
        }
    }
}
