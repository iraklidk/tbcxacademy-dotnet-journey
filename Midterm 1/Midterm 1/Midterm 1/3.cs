using System.Numerics;

public class Midterm_01_03
{
    
    static bool checkVertical(int[,] table, int col, bool turn)
    {
        int nottobe = Convert.ToByte(turn) == 0 ? -1 : 1;
        int k = Convert.ToByte(turn);
        for (int i = 0; i < table.GetLength(0); ++i)
            if (table[i, col] == 0 || table[i, col] == nottobe) return false;

        return true;
    }

    static bool checkHorizontal(int[,] table, int row, bool turn)
    {
        int nottobe = Convert.ToByte(turn) == 0 ? -1 : 1;
        int k = Convert.ToByte(turn);
        for (int i = 0; i < table.GetLength(1); ++i)
            if (table[row, i] == 0 || table[row, i] == nottobe) return false;

        return true;
    }

    static bool checkDiagonal(int[,] table, int row, int col, bool turn)
    {
        int nottobe = Convert.ToByte(turn) == 0 ? -1 : 1;
        int k = Convert.ToByte(turn);
        for(int i = row, j = col; j < table.GetLength(1) && 
                                  i < table.GetLength(0); ++i, ++j)
            if (table[i, j] == 0 || table[i, j] == nottobe) return false;

        for (int i = row, j = col; j >= 0 &&
                          i >= 0; --i, --j)
            if (table[i, j] == 0 || table[i, j] == nottobe) return false;

        for (int i = row, j = col; j < table.GetLength(1) &&
                          i >= 0; --i, ++j)
            if (table[i, j] == 0 || table[i, j] == nottobe) return false;

        for (int i = row, j = col; j >= 0 &&
                          i < table.GetLength(0); ++i, --j)
            if (table[i, j] == 0 || table[i, j] == nottobe) return false;

        return true;
    }

    static bool isValid(int[,] table, int i, int j)
    {
        return i < table.GetLength(0) && i >= 0 && j < table.GetLength(1)
             && j >= 0 && (table[i, j] == 0);
    }

    public static void Main3()
    {
        Console.WriteLine("please insert side length of tictac toe table");
        int k = Convert.ToInt32(Console.ReadLine());

        int[,] table = new int[k, k]; // assume -1 is x n 1 is o
        bool turn = true; // true is mrgvalis turn and false is xs turn
        int count = 0; // total count of turns;
        while (true)
        {
            Console.WriteLine("Please enter row and column");
            int row = Convert.ToInt32(Console.ReadLine());
            int col = Convert.ToInt32(Console.ReadLine());
            bool free = isValid(table, row, col);
            if(free == false)
            {
                    while (free == false) 
                    {
                        Console.WriteLine("please enter correct dimensions: ");
                        row = Convert.ToInt32(Console.ReadLine());
                        col = Convert.ToInt32(Console.ReadLine());
                    if (isValid(table, row, col)) free = true;
                         
                            }
                {
                }
            }
            table[row, col] = turn == false ? -1 : 1;
            ++count;
            turn = !turn;

            if (checkVertical(table, col, turn))
            {
                Console.WriteLine(turn == false ?
                    "moigoo mrgvalmaaa vertikali" : "moigo iqsikma xD sheavso vertikali");
                break;
            }

            if (checkHorizontal(table, row, turn)) {
                Console.WriteLine(turn == false ?
                    "moigoo mrgvalmaaa sheavso horizontali" : "moigo iqsikma xD sheavso horizontali");
                break;
            }

            if (checkDiagonal(table, row, col, turn))
            {
                Console.WriteLine(turn == false ?
                    "moigoo mrgvalmaaa sheavso diagonali" : "moigo iqsikma sheavso diagonali xD");
                break;
            }

            if (count == k * k)
            {
                Console.WriteLine("fre");
                break;
            }
        }
    }
}