using System;
using System.Collections.Generic;
using Project9;

namespace Project9
{
    class Program
    {
        static void Main()
        {
            // Turning Objects
            Leaf leaf = new();
            Pancake pancake = new();
            Corner corner = new();
            Page page = new();

            List<ITurnable> turnables = new List<ITurnable> { leaf, pancake, corner, page };

            Turning(turnables);
        }

        static void Turning(List<ITurnable> t)
        {
            int[] myInts = new int[2];
            int num;

            Console.WriteLine("Enter a number: ");
            num = Convert.ToInt32(Console.ReadLine());

            try
            {
                myInts[num] = 4;
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                t.Add(new Page());
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            foreach (ITurnable turn in t)
            {
                Console.WriteLine(turn.Turn());
            }
        }
    }
}