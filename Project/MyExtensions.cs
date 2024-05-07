using System;

namespace MyExtensions
{
    public static class MyStringExtensions
    {
        public static int MyIndexOf(this string input, string search)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (search == null)
            {
                throw new ArgumentNullException(nameof(search));
            }

            for (int i = 0; i <= input.Length - search.Length; i++)
            {
                bool found = true;

                for (int j = 0; j < search.Length; j++)
                {
                    if (input[i + j] != search[j])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    return i;
                }
            }

            return -1;
        }
        public static bool MyContains<T>(this IEnumerable<T> collection, T item)
        {
            foreach (var element in collection)
            {
                if (EqualityComparer<T>.Default.Equals(element, item))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool MyContains(this string text, string substring)
        {
            return text.IndexOf(substring, StringComparison.Ordinal) >= 0;
        }
    }
}
