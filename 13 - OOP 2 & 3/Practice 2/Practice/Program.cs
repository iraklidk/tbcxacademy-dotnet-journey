namespace Practice
{
    public class Practice_02
    {
        static void PrintArray(int[] array)
        {
            int n = array.Length;
            Console.Write(array[0]);
            if (n == 1) return;
            for (int i = 1; i < n; ++i) Console.Write(", " + array[i]);
        }
        static void Main()
        {

            // string tests
            string str = "raghac stringi", substr = "rag", substr1 = "ingi", substr2 = "test";
            char ch = 'i';
            Console.WriteLine($"Original String: {str}");
            Console.WriteLine($"Reversed String: {str.ReverseString()}");
            Console.WriteLine($"Count of character '{ch}': {str.CountCharacter(ch)}");
            Console.WriteLine($"Starts or ends with '{substr}': " +
                              $"{str.StartOrEndGivenSubstring(substr)}");
            Console.WriteLine($"Starts or ends with '{substr1}': " +
                              $"{str.StartOrEndGivenSubstring(substr1)}");
            Console.WriteLine($"Starts or ends with '{substr2}': " +
                              $"{str.StartOrEndGivenSubstring(substr2)}");
            Console.WriteLine("--------------------------------------\n");

            // numeric tests
            int num1 = 8, num3 = 7;
            int num2 = -13;
            Console.WriteLine($"Number: {num1}, Is Odd: {num1.IsOdd()}\n" +
                              $"Number: {num3}, Is Odd: {num3.IsOdd()}\n" +
                              $"Absolute Value of {num2}: {num2.AbsoluteValue()}");
            int value = 7, multiple = 5;
            Console.WriteLine($"Number: {value}, Rounded to nearest multiple of " +
                              $"{multiple}: {value.RoundToNearestMultiple(multiple)}");
            Console.WriteLine("--------------------------------------\n");

            // data structures test
            int[] arr = { 1, 2, 3, 2, 37, 4, 1, 5 };
            Console.Write("Given array: ");
            PrintArray(arr);
            int[] afterRemoveDuplicate = arr.RemoveDuplicates();
            Console.Write($"\nArray after remove duplicates: ");
            PrintArray(afterRemoveDuplicate);
            Console.WriteLine("\nis 9 in an array? " + (arr.ContainsValue(9) ? "yes" : "no"));
            Console.WriteLine("is 5 in an array? " + (arr.ContainsValue(5) ? "yes" : "no"));
            Console.WriteLine("Max value in an array: " + arr.MaxValueInArray());
            Console.WriteLine("--------------------------------------\n");

            // date time tests
            DateTime date = DateTime.Now;
            Console.WriteLine(date.ToDayString());
            DateTime date1 = new DateTime(2023, 1, 1);
            DateTime date2 = new DateTime(2023, 12, 31);
            DateTime date3 = new DateTime(2027, 6, 15);
            Console.WriteLine($"Is {date} in range {date1} to {date2}: " +
                              $"{date.IsInRange(date1, date2)}");
            Console.WriteLine($"Is {date} in range {date1} to {date3}: " +
                  $"{date.IsInRange(date1, date3)}");
            DateTime birthDate = new DateTime(1990, 11, 24);
            Console.WriteLine("Age based on birth date " + $"{birthDate.ToDayString()}" +
                              $": {birthDate.AgeBasedOnDate()} years");
            Console.WriteLine("--------------------------------------\n");

            // collection test
            int[] array = { 1, 2, 3, 2, 37, 4, 1, 5 };
            int[] anotherArray = { 10, 20, 30, 40, 50 };
            Console.Write("First array: ");
            PrintArray(array);
            Console.Write("\nSecond array: ");
            PrintArray(anotherArray);
            int[] mergedArray = array.MergeSequence(anotherArray);
            Console.Write("\nMerged array: ");
            PrintArray(mergedArray);
            Console.WriteLine();
            Console.WriteLine("--------------------------------------");
        }
    }
}