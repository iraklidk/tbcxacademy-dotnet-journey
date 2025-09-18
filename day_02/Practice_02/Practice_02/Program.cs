Console.WriteLine("Hello, TBC!");

Console.WriteLine("Input the year: ");
int year = Convert.ToInt32(Console.ReadLine());

// using mod operator to check if the year is a leap year

bool iss = (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0); 

Console.WriteLine(!iss ? "False" : "True");