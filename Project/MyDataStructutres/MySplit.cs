public static class MyStringExtensions
{
    public static string[] MySplit(this string input, string delimiter)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        if (delimiter == null)
        {
            throw new ArgumentNullException(nameof(delimiter));
        }

        if (input.Length == 0)
        {
            return new string[0];
        }

        MyList<string> substrings = new MyList<string>();
        int startIndex = 0;

        for (int i = 0; i < input.Length; i++)
        {
            if (IsDelimiterMatch(input, i, delimiter))
            {
                if (i > startIndex)
                {
                    substrings.Add(MySubstring(input, startIndex, i - startIndex));
                }
                startIndex = i + delimiter.Length;
                i += delimiter.Length - 1;
            }
        }

        if (startIndex < input.Length)
        {
            substrings.Add(MySubstring(input, startIndex, input.Length - startIndex));
        }

        return substrings.ToArray();
    }

    private static bool IsDelimiterMatch(string input, int startIndex, string delimiter)
    {
        if (startIndex + delimiter.Length > input.Length)
        {
            return false;
        }

        for (int i = 0; i < delimiter.Length; i++)
        {
            if (input[startIndex + i] != delimiter[i])
            {
                return false;
            }
        }
        return true;
    }

    public static string MySubstring(this string input, int startIndex, int length)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        if (startIndex < 0)
        {
            startIndex = 0;
        }

        if (startIndex >= input.Length)
        {
            return string.Empty;
        }

        if (length <= 0)
        {
            return string.Empty;
        }

        int endIndex = startIndex + length;
        if (endIndex > input.Length)
        {
            endIndex = input.Length;
        }

        char[] result = new char[endIndex - startIndex];

        for (int i = startIndex, j = 0; i < endIndex; i++, j++)
        {
            result[j] = input[i];
        }

        return new string(result);
    }

    public static int MyIndexOf(string input, string search)
    {
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

}
