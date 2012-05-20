using System;
using System.Collections.Generic;
using YouPoorBastard.Model.Extensions;

namespace YouPoorBastard.Model
{
    public class PasswordCracker
    {
        public PasswordCracker(Password passwordToCrack)
        {
            if (passwordToCrack == null)
            {
                throw new ArgumentNullException("passwordToCrack");
            }

            this.PasswordToCrack = passwordToCrack;
        }

        public Password PasswordToCrack { get; private set; }

        public IEnumerable<string> Crack(WordList dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }

            byte[] wordBytes;
            foreach (var word in dictionary.Words)
            {
                wordBytes = word.CalculateVssHash();
                if (this.PasswordToCrack.VssHash.ByteArrayCompare(wordBytes))
                {
                    this.OnPasswordFound(word, wordBytes);
                    yield return word;
                }
            }
        }

        #region events

        public event EventHandler<PasswordFoundEventArgs> PasswordFound;

        private void OnPasswordFound(string password, byte[] hash)
        {
            var tmp = this.PasswordFound;
            if (tmp != null)
            {
                tmp(this, new PasswordFoundEventArgs(password, hash));
            }
        }

        #endregion
    }
}