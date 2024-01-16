using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        List<int> numbers = new List<int>();

        // Loop to gather user-input numbers.
        int userNumber = -1;
        while (userNumber != 0)
        {
            Console.Write("Enter a number (0 to quit): ");
            
            string userResponse = Console.ReadLine();
            userNumber = int.Parse(userResponse);
            
            // Add the entered number to the list.
            if (userNumber != 0)
            {
                numbers.Add(userNumber);
            }
        }

        // Calculating the sum of the numbers.
        int sum = 0;
        foreach (int number in numbers)
        {
            sum += number;
        }
        Console.WriteLine($"The sum is: {sum}");

        // Calculate the average. Casting sum to float.
        float average = ((float)sum) / numbers.Count;
        Console.WriteLine($"The average is: {average}");

        // Finding the maximum number .
        // Initialize max.
        int max = numbers[0];
        foreach (int number in numbers)
        {
            // Update max.
            if (number > max)
            {
                max = number;
            }
        }
        Console.WriteLine($"The max is: {max}");
    }
}
