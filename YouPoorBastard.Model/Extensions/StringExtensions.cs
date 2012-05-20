using System;
using System.Globalization;

namespace YouPoorBastard.Model.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// This is a psuedo-salt used by visual source safe when "hashing" passwords.  Sometimes it's better to accept stupidity than try and understand it.
        /// </summary>
        private const string Nonce = "BrianDavidHarry";

        /// <summary>
        /// Calculates the Visual Source Save 6.0 hash for a string.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>The array of bytes that represents the Visual Source Safe hash.</returns>
        public static byte[] CalculateVssHash(this string password)
        {
            var pwd = string.Empty;

            if (string.IsNullOrEmpty(password))
            {
                pwd = StringExtensions.Nonce;
            }
            else
            {
                pwd = password.ToUpper(CultureInfo.CurrentCulture);
            }

            int passwordLength = password.Length;
            if (passwordLength > 15)
            {
                passwordLength = 15;
            }

            char[] buffer = new char[15];
            for (int i = 0; i < passwordLength; i++)
            {
                buffer[i] = pwd[i];
            }

            int j = 0;
            for (int i = passwordLength; i < 15; i++)
            {
                buffer[i] = StringExtensions.Nonce[j++];
            }

            short hash = 0;
            short multiplier = 1;
            int offset = 0x96;
            short v = 0;
            for (int i = 0; i < 15; i++)
            {
                v = Convert.ToInt16(buffer[i] ^ offset);
                v *= multiplier;
                hash += v;
                multiplier++;
            }

            var bytes = BitConverter.GetBytes(hash);

            return bytes;
        }
    }
}