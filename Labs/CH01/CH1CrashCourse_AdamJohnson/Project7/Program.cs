//Objective:
//Create a Command-Line Interface (CLI) application in C# to manage a
//contact list using a Dictionary<string, string>.
//The application will allow users to add, retrieve,
//update, and delete contacts, where the key is the contact's name and the value
//is their phone number.
//Main Menu: Display a menu with the following options:
//1. Add Contact
//2. View Contact 
//3. Update Contact
//4. Delete Contact 
//5. List All Contacts 
//6. Exit

bool exit = false;
Dictionary<string, string> phoneBook = new();
phoneBook.Add("Adam", "123-456-7890");
phoneBook.Add("Nicole", "098-765-4321");
phoneBook.Add("Jackie", "321-654-0987");

do
{
    Console.WriteLine("\n 1. Add Contact \n 2. View Contact \n 3. Update Contact \n 4. Delete Contact \n 5. List All Contacts \n 6. Exit");
    Console.Write("Enter Choice: ");
    string? choice = Console.ReadLine();
    if (choice.Equals("6"))
    {
        exit = true;
    }
    else if (choice.Equals("1"))
    {
        Console.WriteLine("Enter Name: ");
        string name = Console.ReadLine();
        Console.Write("Enter Phone Number: ");
        string phoneNumber = Console.ReadLine();
        phoneBook.Add(name, phoneNumber);
    }
    else if (choice.Equals("2"))
    {
        Console.WriteLine("Enter Name: ");
        string name = Console.ReadLine();
        if (phoneBook.ContainsKey(name))
        {
            Console.WriteLine($"\n Name: {name} \n Phone Number: {phoneBook[name]}");
        }
        else
        {
            Console.WriteLine("Contact Not Found");
        }
    }
    else if (choice.Equals("3"))
    {
        Console.WriteLine("Enter Name: ");
        string name = Console.ReadLine();
        if (phoneBook.ContainsKey(name))
        {
            Console.Write("Enter Phone Number: ");
            string phoneNumber = Console.ReadLine();
            phoneBook[name] = phoneNumber;
        }
        else
        {
            Console.WriteLine("Contact Not Found");
        }
    }
    else if (choice.Equals("4"))
    {
        Console.Write("Enter Name: ");
        string name = Console.ReadLine();
        if (phoneBook.ContainsKey(name))
        {
            phoneBook.Remove(name);
        }
        else
        {
            Console.WriteLine("Contact Not Found");
        }
    }
    else if (choice.Equals("5"))
    {
        foreach (KeyValuePair<string, string> contact in phoneBook)
        {
            Console.WriteLine($"--------------------\n Name: {contact.Key} \n Phone Number: {contact.Value}");
            Console.WriteLine("---------------------");
        }
    }
} while (exit == false);