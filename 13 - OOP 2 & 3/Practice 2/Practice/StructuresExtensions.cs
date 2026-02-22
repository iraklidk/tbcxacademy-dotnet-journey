namespace Practice
{
    public static class StructuresExtensions
    {
        // Brute force O(n^2) for now, sets/maps not covered yet :)
        public static int[] RemoveDuplicates(this int[] arr)
        {
            int count = 0;
            int n = arr.Length;
            bool[] duplicates = new bool[n];

            for(int i = 0; i < n; ++i)
            {
                if (duplicates[i] == true) continue;
                count++;
                for (int j = i + 1; j < n; ++j)
                {
                    if(arr[i] == arr[j]) duplicates[j] = true;
                }
            }

            int[] result = new int[count];
            int c = 0; // index for result array
            for (int i = 0; i < n; ++i)
                if (duplicates[i] == false) result[c++] = arr[i];

            return result;
        }

        public static bool ContainsValue(this int[] arr, int value)
        {
            foreach (double num in arr) if (num == value) return true;

            return false;
        }

        public static int MaxValueInArray(this int[] arr)
        {
            int mx = int.MinValue;
            foreach (int num in arr) mx = Math.Max(mx, num);
            return mx == int.MinValue ? 0 : mx;
        }
    }
}
