Console.WriteLine("Hello, World!");

Console.WriteLine("Enter the first integer: ");
int a = int.Parse(Console.ReadLine());

Console.WriteLine("Enter the second integer: ");
int b = int.Parse(Console.ReadLine());

Console.WriteLine("before swap: \n" + a + "\n" + b);

// declare temporary variable to hold one value
int tmp = a;
a = b;
b = tmp;

Console.WriteLine("after swap: \n" + a + "\n" + b);

Console.WriteLine();

Console.WriteLine("without tmp variable:\n");
Console.WriteLine("Enter the first integer: ");
int c = int.Parse(Console.ReadLine());

Console.WriteLine("Enter the second integer: ");
int d = int.Parse(Console.ReadLine());

// using exclusive OR
c = c ^ d;
d = c ^ d;
c ^= d;

Console.WriteLine("after swap: \n" + c + "\n" + d);
