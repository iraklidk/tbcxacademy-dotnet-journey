using System.Numerics;

public class Midterm_01_04
{

    static void dfs(string input)
    {
        var files = Directory.GetFiles(input);
        foreach (var file in files)
        {
            Console.WriteLine(file);
            Console.WriteLine("daibechdaaa");
        }

        var dirs = Directory.GetDirectories(input);

        foreach (var dir in dirs)
        {
            dfs(dir);
        }

    }

    public static void Main()
    {
        Console.WriteLine("To turn off program type \"exit\" otherwise " +
            "enter the directory path");

        string input = "";
        while(input != "exit")
        {
            input = Console.ReadLine();
            if (input == "exit") break;

            if (!Directory.Exists(input)) Console.WriteLine($"The directory {input} does not exist!");

            else dfs(input);





        }
    }
}