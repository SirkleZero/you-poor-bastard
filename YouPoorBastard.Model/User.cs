using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace YouPoorBastard.Model
{
    [DebuggerDisplay("Username = {Username}")]
    [StructLayout(LayoutKind.Sequential, Size = 64, Pack = 1, CharSet = CharSet.Ansi)]
    public sealed class User : IEquatable<User>
    {
        #region private fields

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        private string header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        private string checksum;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        private string username;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] passwordHash;

        [MarshalAs(UnmanagedType.I2, SizeConst = 2)]
        private short permissionsFlag;

        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        private int rightsFileOffset;

        private Password password;

        #endregion

        #region constructors

        public User() { }

        #endregion

        #region public properties

        public string Header { get { return this.header; } }
        public string Checksum { get { return this.checksum; } }
        public string Username { get { return this.username; } }
        public short PermissionsFlag { get { return this.permissionsFlag; } }
        public int RightsFileOffset { get { return this.rightsFileOffset; } }

        public Password Password
        {
            get
            {
                if (password == null)
                {
                    this.password = new Password(this.passwordHash);
                }
                return this.password;
            }
        }

        #endregion

        #region IEquatable<VssUser>

        /// <summary>
        ///     <para>Determines if two <see cref="User"/> objects have the same value.</para>
        /// </summary>
        /// <param name="x">The first <see cref="User"/> object to compare.</param>
        /// <param name="y">The second <see cref="User"/> object to compare.</param>
        /// <returns>true if the first <see cref="User"/> is equal to the second <see cref="User"/>; otherwise false.</returns>
        /// <filterpriority>2</filterpriority>
        public static bool Equals(User x, User y)
        {
            if ((object)x == (object)y)
            {
                return true;
            }
            if ((object)x == null || (object)y == null)
            {
                return false;
            }
            if (x.Header.Equals(y.Header) && x.checksum.Equals(y.checksum) && x.username.Equals(y.username) && x.passwordHash.Equals(y.passwordHash) && x.permissionsFlag.Equals(y.permissionsFlag) && x.rightsFileOffset.Equals(y.rightsFileOffset))
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
            return User.Equals(this, objectToCompare as User);
        }

        /// <summary>
        ///     <para>Determines whether the specified <see cref="User"/> is equal to the current <see cref="User"/>.</para>
        /// </summary>
        /// <param name="other">The <see cref="User"/> to compare with the current <see cref="User"/>.</param>
        /// <returns>true if the specified <see cref="User"/> is equal to the current <see cref="User"/>; otherwise false.</returns>
        public bool Equals(User other)
        {
            return User.Equals(this, other);
        }

        /// <summary>
        ///     <para>Determines if two <see cref="User"/> objects have the same value.</para>
        /// </summary>
        /// <param name="x">The first <see cref="User"/> object to compare.</param>
        /// <param name="y">The second <see cref="User"/> object to compare.</param>
        /// <returns>true if the first <see cref="User"/> is equal to the second <see cref="User"/>; otherwise false.</returns>
        public static bool operator ==(User x, User y)
        {
            return User.Equals(x, y);
        }

        /// <summary>
        ///     <para>Determines if two <see cref="User"/> objects have the same value.</para>
        /// </summary>
        /// <param name="x">The first <see cref="User"/> object to compare.</param>
        /// <param name="y">The second <see cref="User"/> object to compare.</param>
        /// <returns>false if the first <see cref="User"/> is equal to the second <see cref="User"/>; otherwise true.</returns>
        public static bool operator !=(User x, User y)
        {
            return !User.Equals(x, y);
        }

        #endregion

        /// <summary>
        ///     <para>Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"/> is suitable for use in hashing algorithms and data structures like a hash table.</para>
        /// </summary>
        /// <returns>A hash code for the current <see cref="T:System.Object"/>.</returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return this.Header.GetHashCode() ^ this.checksum.GetHashCode() ^ this.username.GetHashCode() ^ this.passwordHash.GetHashCode() ^ this.permissionsFlag.GetHashCode() ^ this.rightsFileOffset.GetHashCode();
        }
    }
}