internal class Tuples
{
    static void GetMinMax(int[] nums, out int min, out int max)
    {
        min = int.MaxValue;
        max = int.MinValue;

        foreach (var item in nums)
        {
            min = Math.Min(item, min);
            max = Math.Max(item, max);
        }
    }

    static (int min, int max) GetMinMaxTuple(int[] nums)
    {
        int min = int.MaxValue;
        int max = int.MinValue;

        foreach (var item in nums)
        {
            min = Math.Min(item, min);
            max = Math.Max(item, max);
        }

        return (min, max);
    }

    static void Main3()
    {
        int[] numbers = { 3, 1, 9, 5 };
        foreach(var item in numbers) Console.Write(item + " ");
        Console.WriteLine("");
        var (min, max) = GetMinMaxTuple(numbers);
        Console.WriteLine($"Min = {min}, Max = {max}");
    }
}