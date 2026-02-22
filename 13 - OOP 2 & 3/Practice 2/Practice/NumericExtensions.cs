namespace Practice
{
    public static class NumericExtensions
    {
        public static int RoundToNearestMultiple(this int value, int multiple)
        {
            if (multiple == 0) return value;

            return ((double)(value % multiple) >= multiple / 2.0) ? 
           multiple * ((value + multiple - 1) / multiple) : multiple * (value / multiple);
        }

        public static bool IsOdd(this int i)
        {
            return (i & 1) == 1;
        }

        public static int AbsoluteValue(this int i)
        {
            return i < 0 ? -i : i;
        }
    }
}