//Multiplication Chart Lab
int rowNumber, colNumber;
Console.WriteLine("Welcome to the Multiplication Chart Application!\n");
Console.WriteLine("How many rows should this multiplication chart have?");
while (true)
{
    if (int.TryParse(Console.ReadLine(), out rowNumber)) break;
    Console.WriteLine("\nPlease enter a valid integer for rows.");
}

Console.WriteLine("\nHow many columns should this multiplication chart have?");
while (true)
{
    if (int.TryParse(Console.ReadLine(), out colNumber)) break;
    Console.WriteLine("\nPlease enter a valid integer for columns.");
}

Console.WriteLine($"\nHere is your multiplication chart with {rowNumber} rows and {colNumber} columns!\n");

Console.Write("    ");
for (int j = 1; j <= colNumber; j++)
{
    Console.Write($" {j,4} |");
}
Console.WriteLine();

Console.Write("    ");
for (int j = 1; j <= colNumber; j++)
{
    Console.Write("-----");
}
Console.WriteLine();

for (int i = 1; i <= rowNumber; i++)
{
    Console.Write($"{i,2} |");
    for (int j = 1; j <= colNumber; j++)
    {
        Console.Write($" {i * j,4} |");
    }
    Console.WriteLine();
}