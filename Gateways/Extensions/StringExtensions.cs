using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.Extensions
{
    public static class StringExtensions
    {
        public static string TrimLengthWithEllipsis([NotNull] this string str, int maxLength)
        {
            if (str.Length <= maxLength)
            {
                return str;
            }

            return str.Substring(0, maxLength) + "...";
        }

        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
