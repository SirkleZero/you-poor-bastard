using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace YouPoorBastard.Model.Extensions
{
    /// <summary>
    /// Provides extension methods to both <see cref="IEnumerable"/> and <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Cycles through a collection of items and wraps the item with descriptive generic typed meta information or functionality.
        /// </summary>
        /// <typeparam name="T">The type of generic <see cref="{T}"/> being wrapped.</typeparam>
        /// <typeparam name="TIdentifier">The type of generic <see cref="{TIdentifier}"/> that represents the meta information type wrapping the target.</typeparam>
        /// <param name="items">The <see cref="IEnumerable{T}"/> that will be cycled through.</param>
        /// <param name="identifiers">The array of parameters that will be attached as meta information or functionality to the enumerated objects.</param>
        /// <returns>An <see cref="IEnumerable{ICycledItem{T, TIdentifier}}"/> that contains the original wrapped items, as well as the meta information or functionality.</returns>
        public static IEnumerable<ICycledItem<T, TIdentifier>> Cycle<T, TIdentifier>(this IEnumerable<T> items, params TIdentifier[] identifiers)
        {
            // turn this into an array if it isn't already.  we need to avoid unnecessary conversion.
            T[] array = null;
            if (items is Array)
            {
                array = items as T[];
            }
            else
            {
                array = items.ToArray();
            }

            for (int i = 0; i < array.Length; i++)
            {
                TIdentifier identifier = default(TIdentifier);
                if (identifiers != null && identifiers.Length > 0)
                {
                    identifier = identifiers[i % identifiers.Length];
                }
                yield return new CycledItem<T, TIdentifier>(array[i], identifier);
            }
        }

        /// <summary>
        /// Creates a delimeted string from a collection object of type <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of object the collection contains.</typeparam>
        /// <param name="collection">The collection that contains the data to be exported.</param>
        /// <param name="options">The <see cref="DelimetedOptions"/> used to format the data exported.</param>
        /// <returns>A string derived from the <see cref="IEnumerable{T}"/> collection in CSV Format.</returns>
        /// <exception cref="ArgumentNullException">
        /// 	<para>The argument <paramref name="options"/> is <langword name="null"/>.</para>
        /// </exception>
        public static string ToDelimetedString<T>(this IEnumerable<T> collection, DelimetedOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            if (collection.Count() > 0)
            {
                var builder = new StringBuilder();

                PropertyInfo[] properties = null;
                object value = null;
                var isFirst = true;

                #region header creation

                if (options.IncludeHeader)
                {
                    var headerProperties = collection.First().GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var property in headerProperties)
                    {
                        if (!isFirst)
                        {
                            builder.Append(options.Delimeter);
                        }
                        builder.Append(property.Name);
                        isFirst = false;
                    }
                    builder.AppendLine();
                    isFirst = true;
                }

                #endregion

                #region file body creation

                foreach (var item in collection)
                {
                    if (item.GetType().IsPrimitive)
                    {
                        if (!isFirst)
                        {
                            builder.Append(options.Delimeter);
                        }
                        builder.Append(EnumerableExtensions.EscapeString(item.ToString()));
                        isFirst = false;
                    }
                    else
                    {
                        properties = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        foreach (var property in properties)
                        {
                            if (!isFirst)
                            {
                                builder.Append(options.Delimeter);
                            }
                            value = property.GetValue(item, null);
                            builder.Append(EnumerableExtensions.EscapeString(value.ToString()));
                            isFirst = false;
                        }
                    }
                    builder.AppendLine();
                    isFirst = true;
                }

                #endregion

                return builder.ToString();
            }
            return null;
        }

        /// <summary>
        /// Creates a CSV string from a collection object of type <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of object the collection contains.</typeparam>
        /// <param name="collection">The collection that contains the data to be exported.</param>
        /// <param name="options">The <see cref="DelimetedOptions"/> used to format the data exported.</param>
        /// <returns>A string derived from the <see cref="IEnumerable{T}"/> collection in CSV Format.</returns>
        public static string ToCsvString<T>(this IEnumerable<T> collection)
        {
            return collection.ToDelimetedString(new DelimetedOptions());
        }

        #region private methods

        /// <summary>
        /// Escapes a string based on CSV string formatting rules.
        /// </summary>
        /// <param name="s">The string to format.</param>
        /// <returns>The original string, formatted for inclusion in CSV output.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 	<para>The argument <paramref name="s"/> is out of range.</para>
        /// </exception>
        private static string EscapeString(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            // see if the string contains a comma
            // see if the string contains double quotes
            // see if the field has line breaks
            // see if the string has leading or trailing whitespace
            var containsComma = s.Contains(',');
            var containsDoubleQuotes = s.Contains('"');
            var containsLineBreak = s.Contains(Environment.NewLine);
            var containsLeadingOrTrailingWhitespace = s.StartsWith(" ") || s.EndsWith(" ");

            if (containsDoubleQuotes)
            {
                // replace double quotes with double, double quotes
                s = s.Replace("\"", "\"\"");
            }
            if (containsComma || containsDoubleQuotes || containsLineBreak || containsLeadingOrTrailingWhitespace)
            {
                // if any above are true, we need to qualify the string.
                s = string.Concat("\"", s, "\"");
            }

            return s;
        }

        #endregion
    }

    #region objects used by EnumerableExtensions

    /// <summary>
    /// Provides an interface that is used to wrap an object with meta information from a Cycle operation.
    /// </summary>
    /// <typeparam name="T">The type of generic <see cref="{T}"/> being wrapped.</typeparam>
    /// <typeparam name="TIdentifier">The type of generic <see cref="{TIdentifier}"/> that represents the meta information type wrapping the target.</typeparam>
    public interface ICycledItem<T, TIdentifier>
    {
        /// <summary>
        /// Gets the generic item of type <see cref="{T}"/> that is wrapped.
        /// </summary>
        T Item { get; }
        /// <summary>
        /// Gets the identifying meta information attached to the wrapped object.
        /// </summary>
        TIdentifier Identifier { get; }
    }

    /// <summary>
    /// A wrapper class that provides meta information to an object of a Cycle operation.
    /// </summary>
    /// <typeparam name="T">The type of generic <see cref="{T}"/> being wrapped.</typeparam>
    /// <typeparam name="TIdentifier">The type of generic <see cref="{TIdentifier}"/> that represents the meta information type wrapping the target.</typeparam>
    public sealed class CycledItem<T, TIdentifier> : ICycledItem<T, TIdentifier>
    {
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="CycledItem"/> class.</para>
        /// </summary>
        /// <param name="item">The generic item of type <see cref="{T}"/> that is wrapped.</param>
        /// <param name="identifier">The identifying meta information attached to the wrapped object.</param>
        /// <exception cref="ArgumentNullException">
        /// 	<para>The argument <paramref name="item"/> is <langword name="null"/>.</para>
        /// 	<para>-or-</para>
        /// 	<para>The argument <paramref name="identifier"/> is <langword name="null"/>.</para>
        /// </exception>
        public CycledItem(T item, TIdentifier identifier)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (identifier == null)
            {
                throw new ArgumentNullException("identifier");
            }
            this.Item = item;
            this.Identifier = identifier;
        }

        #region CycledItem<T> Members

        /// <summary>
        /// Gets the generic item of type <see cref="{T}"/> that is wrapped.
        /// </summary>
        public T Item { get; private set; }

        /// <summary>
        /// Gets the identifying meta information attached to the wrapped object.
        /// </summary>
        public TIdentifier Identifier { get; private set; }

        #endregion
    }

    /// <summary>
    /// An object that provides optional configuration information for <see cref="IEnumerable{T}.ToDelimetedFile{T}"/>.
    /// </summary>
    public sealed class DelimetedOptions
    {
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="DelimetedOptions"/> class.</para>
        /// </summary>
        public DelimetedOptions() : this(',', false) { }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="DelimetedOptions"/> class.</para>
        /// </summary>
        /// <param name="delimeter">The character that will be used to delimit data in the file.</param>
        /// <param name="includeHeader">Specifies if a column header will be included in the file.</param>
        public DelimetedOptions(char delimeter, bool includeHeader)
        {
            this.Delimeter = delimeter;
            this.IncludeHeader = includeHeader;
        }

        /// <summary>
        /// Gets the character that will be used to delimit data in the file.
        /// </summary>
        public char Delimeter { get; private set; }

        /// <summary>
        /// Gets a value that specifies if a column header will be included in the file.
        /// </summary>
        public bool IncludeHeader { get; private set; }
    }

    #endregion
}