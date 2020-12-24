using System;
namespace ArtScanner.Utils.Helpers
{
    static class StringExtension
    {
        public static string OnlyDigits(this string str)
        {
            string result = string.Empty;

            foreach (var c in str)
            {
                if (Char.IsDigit(c))
                {
                    result += c;
                }
            }

            return result;
        }
    }
}
