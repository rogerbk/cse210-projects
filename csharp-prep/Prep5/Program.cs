using System;

class Program
{
    static void Main(string[] args)
    {
        DisplayWelcomeMessage(); // welcome message.

        string userName = PromptUserName(); // read the user's name.
        int userNumber = PromptUserNumber(); // read the user's favorite number.

        int squaredNumber = SquareNumber(userNumber); // square the user's number.

        DisplayResult(userName, squaredNumber); // display the result to the user.
    }

       static void DisplayWelcomeMessage()
    {
        Console.WriteLine("Welcome to the program!");
    }

    static string PromptUserName()
    {
        Console.Write("Please enter your name: ");
        string name = Console.ReadLine();

        return name;
    }

    static int PromptUserNumber()
    {
        Console.Write("Please enter your favorite number: ");
        int number = int.Parse(Console.ReadLine());

        return number;
    }

    static int SquareNumber(int number)
    {
        int square = number * number;
        return square;
    }

    static void DisplayResult(string name, int square)
    {
        Console.WriteLine($"{name}, the square of your number is {square}");
    }
}
