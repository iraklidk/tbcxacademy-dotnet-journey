namespace Practice
{
    public static class StringExtensions
    {
        public static string ReverseString(this string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static int CountCharacter(this string str, char ch)
        {
            int count = 0;
            foreach(char c in str) if (c == ch) ++count;
            return count;
        }

        public static bool StartOrEndGivenSubstring(this string str, string substr)
        {
            int n = substr.Length;
            if(str.Length < n) return false;
            return str.Substring(0, n) == substr || str.Substring(str.Length - n) == substr;
        }
    }
}
