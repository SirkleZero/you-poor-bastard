using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace YouPoorBastard.Model
{
    public class Database : IEquatable<Database>
    {
        public Database(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentOutOfRangeException("path");
            }
            if (!File.Exists(Path.Combine(path, "um.dat")))
            {
                throw new FileNotFoundException("The users file for the vss database specified does not exist. Make sure the path specified is a valid source safe path.");
            }

            this.DatabasePath = path;
        }

        public string DatabasePath { get; private set; }

        public User GetUser(string username)
        {
            try
            {
                return this.GetUsers().Single(m => m.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<User> GetUsers()
        {
            var users = new List<User>();
            var umPath = Path.Combine(this.DatabasePath, "um.dat");

            var reader = new BinaryReader(File.Open(umPath, FileMode.Open));

            try
            {
                var testArray = new byte[3];
                int count = reader.Read(testArray, 0, 3);

                if (!count.Equals(0))
                {
                    #region file format documentation

                    //The um.dat file
                    //This is the user management file, and has the following contents: 
                    //0x0000: ID String = "UserManagement@Microsoft"
                    //0x0038: "HU"
                    //0x003c: Number of VSS users recorded (2-byte int)
                    //0x007c: Length of next user record (4-byte int)
                    //0x0080: User record is as layed out below (Usually 64 bytes, except for last record in file which is 60 bytes because the last field is absent)

                    //User record: 
                    //0x0000: "UU"
                    //0x0002: Checksum of this record (2 bytes) (Must be recomputed if anything changes in user record otherwise VSS reports um.dat is corrupt)
                    //0x0004: User name (32 bytes, including zero terminator) (e.g. "Admin")
                    //0x0024: Hash of password for this user or 0x90 0x6e if no password (2 bytes)
                    //0x0026: Permissions flag: 0x00=Read-Write, 0x01=Read-Only
                    //0x0028: Offset of 148 byte record for this user in the file rights.dat (4 bytes)
                    //0x003c: Length of next user record (4-byte int)

                    #endregion

                    // seek to where the users start in the file.
                    reader.BaseStream.Seek(0x003c, SeekOrigin.Begin);
                    var userCount = reader.ReadInt16();

                    reader.BaseStream.Seek(0x0080, SeekOrigin.Begin);
                    for (var i = 0; i < userCount; i++)
                    {
                        var buffer = reader.ReadBytes(64);
                        var result = Database.RawDataToObject<User>(ref buffer);
                        users.Add(result);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} caught and ignored. Using default values.", e.GetType().Name);
            }
            finally
            {
                reader.Close();
            }

            return users;
        }

        private static T RawDataToObject<T>(ref byte[] buffer) where T : class
        {
            T result = null;
            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                result = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T)) as T;
            }
            finally
            {
                handle.Free();
            }

            return result;
        }

        #region IEquatable<VssDatabase>

        /// <summary>
        ///     <para>Determines if two <see cref="Database"/> objects have the same value.</para>
        /// </summary>
        /// <param name="x">The first <see cref="Database"/> object to compare.</param>
        /// <param name="y">The second <see cref="Database"/> object to compare.</param>
        /// <returns>true if the first <see cref="Database"/> is equal to the second <see cref="Database"/>; otherwise false.</returns>
        /// <filterpriority>2</filterpriority>
        public static bool Equals(Database x, Database y)
        {
            if ((object)x == (object)y)
            {
                return true;
            }
            if ((object)x == null || (object)y == null)
            {
                return false;
            }
            if (x.DatabasePath.Equals(y.DatabasePath))
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
            return Database.Equals(this, objectToCompare as Database);
        }

        /// <summary>
        ///     <para>Determines whether the specified <see cref="Database"/> is equal to the current <see cref="Database"/>.</para>
        /// </summary>
        /// <param name="other">The <see cref="Database"/> to compare with the current <see cref="Database"/>.</param>
        /// <returns>true if the specified <see cref="Database"/> is equal to the current <see cref="Database"/>; otherwise false.</returns>
        public bool Equals(Database other)
        {
            return Database.Equals(this, other);
        }

        /// <summary>
        ///     <para>Determines if two <see cref="Database"/> objects have the same value.</para>
        /// </summary>
        /// <param name="x">The first <see cref="Database"/> object to compare.</param>
        /// <param name="y">The second <see cref="Database"/> object to compare.</param>
        /// <returns>true if the first <see cref="Database"/> is equal to the second <see cref="Database"/>; otherwise false.</returns>
        public static bool operator ==(Database x, Database y)
        {
            return Database.Equals(x, y);
        }

        /// <summary>
        ///     <para>Determines if two <see cref="Database"/> objects have the same value.</para>
        /// </summary>
        /// <param name="x">The first <see cref="Database"/> object to compare.</param>
        /// <param name="y">The second <see cref="Database"/> object to compare.</param>
        /// <returns>false if the first <see cref="Database"/> is equal to the second <see cref="Database"/>; otherwise true.</returns>
        public static bool operator !=(Database x, Database y)
        {
            return !Database.Equals(x, y);
        }

        #endregion

        /// <summary>
        ///     <para>Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"/> is suitable for use in hashing algorithms and data structures like a hash table.</para>
        /// </summary>
        /// <returns>A hash code for the current <see cref="T:System.Object"/>.</returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return this.DatabasePath.GetHashCode();
        }
    }
}