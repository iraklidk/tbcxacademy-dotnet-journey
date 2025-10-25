namespace Practice
{
    public static class QueueTests
    {

        public static void RunTests()
        {
            Console.WriteLine("\n====================QUEUE====================");

            var queue = new GenericQueue<int>();

            Console.WriteLine("\n=== Dequeue from empty queue ===");
            try { queue.Dequeue(); }
            catch (Exception ex) { Console.WriteLine("Exception: " + ex.Message); } // queeue is empty

            Console.WriteLine("\n=== Peek from empty queue ===");
            try { queue.Peek(); }
            catch (Exception ex) { Console.WriteLine("Exception: " + ex.Message); } // queue is empty

            Console.WriteLine("\n=== Enqueue 10 20 30 40 ===");
            queue.Enqueue(10);
            queue.Enqueue(20);
            queue.Enqueue(30);
            queue.Enqueue(40);

            Console.WriteLine("\n=== Circular wrap-around ===");
            Console.WriteLine($"dequeue: " + queue.Dequeue()); // 10
            Console.WriteLine($"dequeue: " + queue.Dequeue()); // 20
            Console.WriteLine("\nEnqueue: 50 60");
            queue.Enqueue(50); // wraps to start
            queue.Enqueue(60); // fills tail again
            Console.WriteLine($"dequeue: " + queue.Dequeue()); // 30
            Console.WriteLine($"Queue size: " + queue.Length); // 3
            Console.WriteLine($"\nPeek now: {queue.Peek()}"); // 40
            Console.WriteLine($"dequeue: " + queue.Dequeue()); // 40
            Console.WriteLine($"dequeue: " + queue.Dequeue()); // 50

            Console.WriteLine("\n=== Peek after wrap-around ===");
            Console.WriteLine("\nEnqueue 70 80 ===");
            queue.Enqueue(70);
            queue.Enqueue(80);
            Console.WriteLine("Peek now: " + queue.Peek()); // 60
            Console.WriteLine("\n=== Contains ===");
            Console.WriteLine($"Containts 60: " + queue.Contains(60)); // False
            Console.WriteLine("dequeue: " + queue.Dequeue()); // 60
            Console.WriteLine($"Containts 60: " + queue.Contains(60)); // True
            Console.WriteLine("\n dequeue: " + queue.Dequeue()); // 70
            Console.WriteLine($"Queue size: " + queue.Length); // 1
            Console.WriteLine("Peek Nnow: " + queue.Peek()); // 80
            Console.WriteLine($"dequeue: " + queue.Dequeue()); // 70
            Console.WriteLine($"Queue size: " + queue.Length); // 0


            Console.WriteLine("\n=== Single element queue ===");
            queue.Clear();
            Console.WriteLine("\nEnqueue: 90");
            queue.Enqueue(90);
            Console.WriteLine("Peek now: " + queue.Peek());    // 90
            Console.WriteLine("dequeue: " + queue.Dequeue()); // 90
            Console.WriteLine($"Queue size: " + queue.Length); // 0
        }

    }
}