// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello and welcome to AWD 1115!"); //Welcome line

Console.WriteLine("Please enter your name:"); //Asks the user for their name.
string name = Console.ReadLine();

Console.WriteLine("Please enter your age:"); //Asks the user for their age.
int userAge = Convert.ToInt32(Console.ReadLine());

Console.WriteLine($"Hi {name}, you are {userAge} years old.");