namespace Practice
{
    public static class BSTTests
    {
        public static void RunTests()
        {
            Console.WriteLine("\n====================BST====================");

            var bst = new GenericBST<int>();

            Console.WriteLine("=== Search in empty tree ===");
            Console.WriteLine(bst.Search(10) == null ? "Not found" : "Found"); // Not found

            Console.WriteLine("\n=== Insert values ===");
            Console.WriteLine("Insert: 10, 5, 15, 10, 12, 20");
            bst.Insert(10);
            bst.Insert(5);
            bst.Insert(15);
            bst.Insert(10); // duplicate
            bst.Insert(12);
            bst.Insert(20);

            Console.WriteLine("\n=== Search existing values ===");
            Console.WriteLine(bst.Search(10) != null ? "Found 10" : "Not found"); // Found 10
            Console.WriteLine(bst.Search(5) != null ? "Found 5" : "Not found");   // Found 5
            Console.WriteLine(bst.Search(20) != null ? "Found 20" : "Not found"); // Found 20

            Console.WriteLine("\n=== Search non-existing values ===");
            Console.WriteLine(bst.Search(7) == null ? "Not found 7" : "Found 7");  // Not found
            Console.WriteLine(bst.Search(30) == null ? "Not found 30" : "Found 30"); // Not found

            Console.WriteLine("\n=== Delete occurrence ===");
            bst.Delete(10);
            Console.WriteLine(bst.Search(10) == null ? "10 deleted" : "10 still exists"); // 10 deleted
            Console.WriteLine(bst.Search(5) == null ? "5 deleted" : "5 still exists"); // 5 still exists

            Console.WriteLine("\n=== Delete non-existing value ===");
            bst.Delete(100); // should do nothing
            Console.WriteLine("Deleted 100? No crash, OK!");

            Console.WriteLine("\n=== Search after multiple deletions ===");
            Console.WriteLine(bst.Search(12) != null ? "Found 12" : "Not found"); // Found 12
            Console.WriteLine(bst.Search(15) != null ? "Found 15" : "Not found"); // Found 15
            Console.WriteLine(bst.Search(20) != null ? "Found 20" : "Not found"); // Found 20

            bst.Insert(20); bst.Insert(20); bst.Insert(20);
            Console.WriteLine("\n=== PRINT BST IN IN-ORDER TRAVERSAL ===");
            bst.PrintBSTAsSortedArray();

            var bst1 = new GenericBST<string>();

            Console.WriteLine("\n\n=== String BST Test ===");
            Console.WriteLine("Insert: mango, apple, banana, peach (respectively)");
            bst1.Insert("mango");
            bst1.Insert("apple");
            bst1.Insert("banana");
            bst1.Insert("peach");

            Console.Write("In-order traversal (should be sorted): ");
            bst1.PrintBSTAsSortedArray(); // apple, banana, mango, peach

            Console.WriteLine("\n\n=== Search tests ===");
            Console.WriteLine(bst1.Search("banana") != null ? "Found banana" : "Not found banana");
            Console.WriteLine(bst1.Search("cherry") != null ? "Found cherry" : "Not found cherry");

            Console.WriteLine("\n=== Delete test ===");
            bst1.Delete("banana");
            Console.WriteLine(bst1.Search("banana") == null ? "banana deleted" : "banana still exists");

            Console.Write("\nAfter deletion (should be: apple, mango, peach): ");
            bst1.PrintBSTAsSortedArray();

            Console.WriteLine("\n\n\n");
        }
    }
}