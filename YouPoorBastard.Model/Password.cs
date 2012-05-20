using System;
using System.Collections.Generic;

namespace YouPoorBastard.Model
{
    public class Password
    {
        public Password(byte[] vssHash)
        {
            if (vssHash == null)
            {
                throw new ArgumentNullException("vssHash");
            }

            this.VssHash = vssHash;
        }

        public byte[] VssHash { get; private set; }

        public IEnumerable<string> Crack(WordList dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }

            var cracker = new PasswordCracker(this);
            return cracker.Crack(dictionary);
        }
    }
}