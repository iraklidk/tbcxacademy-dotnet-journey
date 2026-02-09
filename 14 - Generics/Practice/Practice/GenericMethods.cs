namespace Practice
{
    public static class GenericMethods
    {

        public static void GenericSwap<T>(T[] array, int a,  int b)
        {
            {
                int size = array.Length;
                if(a < 0 || a >= size || b < 0 || b >= size) throw new
                        ArgumentOutOfRangeException("Index out of range");
                T temp = array[a];
                array[a] = array[b];
                array[b] = temp;
            }
        }

        public static T GenericMaxElement<T>(T[] array) where T : IComparable<T>
        {
            if(array.Length == 0) throw new ArgumentException("Array is empty");
            T max = array[0];
            for(int i = 1; i < array.Length; ++i) if(array[i].CompareTo(max) > 0) max = array[i];
            
            return max;
        }

        public static void GenericPrintArray<T>(T[] array)
        {
            foreach(var elem in array) Console.Write(elem + " ");

            Console.WriteLine();
        }

    }
}