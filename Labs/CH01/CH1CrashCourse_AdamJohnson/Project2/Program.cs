// Fizz Buzz Program
//RULES:
//Write a program that prints numbers from 1 to N (given upper limit), but:
//For numbers divisible of 3, print "Fizz" instead of the number.
//For numbers divisible of 5, print "Buzz" instead of the number.
//For numbers divisible of both 3 and 5, print "FizzBuzz" instead of the number.

int upperLimit = 100;
Console.WriteLine("What is the upper limit of our Fizz Buzz?");
int.TryParse(Console.ReadLine(), out upperLimit);

for (int i = 1; i <= upperLimit; i++)
{
    if (i % 3 == 0 && i % 5 == 0)
    {
        Console.WriteLine("FizzBuzz");
    }
    else if (i % 3 == 0)
    {
        Console.WriteLine("Fizz");
    }
    else if (i % 5 == 0)
    {
        Console.WriteLine("Buzz");
    }
    else
    {
        Console.WriteLine($"{i:F3}");
    }
}

float n = .12f;

Console.WriteLine($"{n:P0}");

Console.WriteLine("The value is " + n.ToString("P0"));