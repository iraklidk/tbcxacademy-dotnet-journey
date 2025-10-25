namespace Practice
{
    public static class StackTests
    {

        public static void RunTests()
        {
            Console.WriteLine("\n====================STACK====================\n");

            var stringStack = new GenericStack<string>();

            //  push strings
            Console.WriteLine("===PUSH===");
            Console.WriteLine("PUSH: Hello, World, Stack");
            stringStack.Push("Hello");
            stringStack.Push("World");
            stringStack.Push("Stack");

            Console.WriteLine("Top element (Peek): " + stringStack.Peek()); // Stack

            // pop strings
            Console.WriteLine("\n===POP===");
            Console.WriteLine("Pop: " + stringStack.Pop()); // Stack
            Console.WriteLine("Pop: " + stringStack.Pop()); // World

            // contains check
            Console.WriteLine("\n===CONTSINS===");
            Console.WriteLine("Contains 'Hello'? " + stringStack.Contains("Hello")); // True
            Console.WriteLine("Contains 'Stack'? " + stringStack.Contains("Stack")); // False

            // pop last element
            Console.WriteLine("\n===POP LAST===");
            Console.WriteLine("Pop: " + stringStack.Pop()); // Hello

            // pop from empty stack
            Console.WriteLine("\n===POP FROM EMPTY STACK===");
            try { stringStack.Pop(); }

            catch (Exception ex) { Console.WriteLine("Exception: " + ex.Message); } // stack is empty

            // push after emptying
            Console.WriteLine("\nPUSH: New, String");
            stringStack.Push("New");
            stringStack.Push("String");
            Console.WriteLine("Peek: " + stringStack.Peek()); // String
        }

    }
}