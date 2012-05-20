using System;

namespace YouPoorBastard.Model
{
    public class PasswordFoundEventArgs : EventArgs, IEquatable<PasswordFoundEventArgs>
    {
        public PasswordFoundEventArgs(string password, byte[] hash)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentOutOfRangeException("password");
            }
            if (hash == null)
            {
                throw new ArgumentNullException("hash");
            }

            this.Password = password;
            this.Hash = hash;
        }

        public byte[] Hash { get; set; }
        public string Password { get; set; }

        #region IEquatable<PasswordFoundEventArgs>

        /// <summary>
        ///     <para>Determines if two <see cref="PasswordFoundEventArgs"/> objects have the same value.</para>
        /// </summary>
        /// <param name="x">The first <see cref="PasswordFoundEventArgs"/> object to compare.</param>
        /// <param name="y">The second <see cref="PasswordFoundEventArgs"/> object to compare.</param>
        /// <returns>true if the first <see cref="PasswordFoundEventArgs"/> is equal to the second <see cref="PasswordFoundEventArgs"/>; otherwise false.</returns>
        /// <filterpriority>2</filterpriority>
        public static bool Equals(PasswordFoundEventArgs x, PasswordFoundEventArgs y)
        {
            if ((object)x == (object)y)
            {
                return true;
            }
            if ((object)x == null || (object)y == null)
            {
                return false;
            }
            if (x.Password.Equals(y.Password))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     <para>Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.</para>
        /// </summary>
        /// <param name="objectToCompare">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise false.</returns>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object objectToCompare)
        {
            return PasswordFoundEventArgs.Equals(this, objectToCompare as PasswordFoundEventArgs);
        }

        /// <summary>
        ///     <para>Determines whether the specified <see cref="PasswordFoundEventArgs"/> is equal to the current <see cref="PasswordFoundEventArgs"/>.</para>
        /// </summary>
        /// <param name="other">The <see cref="PasswordFoundEventArgs"/> to compare with the current <see cref="PasswordFoundEventArgs"/>.</param>
        /// <returns>true if the specified <see cref="PasswordFoundEventArgs"/> is equal to the current <see cref="PasswordFoundEventArgs"/>; otherwise false.</returns>
        public bool Equals(PasswordFoundEventArgs other)
        {
            return PasswordFoundEventArgs.Equals(this, other);
        }

        /// <summary>
        ///     <para>Determines if two <see cref="PasswordFoundEventArgs"/> objects have the same value.</para>
        /// </summary>
        /// <param name="x">The first <see cref="PasswordFoundEventArgs"/> object to compare.</param>
        /// <param name="y">The second <see cref="PasswordFoundEventArgs"/> object to compare.</param>
        /// <returns>true if the first <see cref="PasswordFoundEventArgs"/> is equal to the second <see cref="PasswordFoundEventArgs"/>; otherwise false.</returns>
        public static bool operator ==(PasswordFoundEventArgs x, PasswordFoundEventArgs y)
        {
            return PasswordFoundEventArgs.Equals(x, y);
        }

        /// <summary>
        ///     <para>Determines if two <see cref="PasswordFoundEventArgs"/> objects have the same value.</para>
        /// </summary>
        /// <param name="x">The first <see cref="PasswordFoundEventArgs"/> object to compare.</param>
        /// <param name="y">The second <see cref="PasswordFoundEventArgs"/> object to compare.</param>
        /// <returns>false if the first <see cref="PasswordFoundEventArgs"/> is equal to the second <see cref="PasswordFoundEventArgs"/>; otherwise true.</returns>
        public static bool operator !=(PasswordFoundEventArgs x, PasswordFoundEventArgs y)
        {
            return !PasswordFoundEventArgs.Equals(x, y);
        }

        #endregion

        /// <summary>
        ///     <para>Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"/> is suitable for use in hashing algorithms and data structures like a hash table.</para>
        /// </summary>
        /// <returns>A hash code for the current <see cref="T:System.Object"/>.</returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return this.Password.GetHashCode();
        }
    }
}