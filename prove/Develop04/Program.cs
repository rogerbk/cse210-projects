//As part of the exceeding challege I added this:Keeping a log of how many times activities were performed.

using System;
using System.Collections.Generic;
using System.Threading;

abstract class MindfulnessActivity
{
    protected int Duration;
    private string ActivityName;

    protected MindfulnessActivity(string name)
    {
        ActivityName = name;
    }

    public void StartActivity()
    {
        ShowStartingMessage();
        SetDuration();
        PrepareToBegin();
        RunActivity();
        EndActivity();
        Program.LogActivity(this.GetType().Name);
    }

    protected abstract void RunActivity();

    private void ShowStartingMessage()
    {
        Console.WriteLine($"Welcome to the {ActivityName}.");
        Console.WriteLine("Please enter the duration of the activity in seconds:");
    }

    private void SetDuration()
    {
        Duration = int.Parse(Console.ReadLine());
    }

    private void PrepareToBegin()
    {
        Console.WriteLine("Get ready...");
        Countdown(5);
    }

    private void EndActivity()
    {
        Console.WriteLine("You have done a good job!");
        Countdown(3);
        Console.WriteLine($"You have completed the {ActivityName} for {Duration} seconds.");
        Countdown(3);
    }

    protected void Countdown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write(i + " ");
            Thread.Sleep(1000);
        }
        Console.WriteLine();
    }
}

class BreathingActivity : MindfulnessActivity
{
    public BreathingActivity() : base("Breathing Activity") { }

    protected override void RunActivity()
    {
        int halfDuration = Duration / 2;
        for (int i = 0; i < halfDuration; i++)
        {
            Console.WriteLine("Breathe in...");
            Countdown(2);
            Console.WriteLine("Breathe out...");
            Countdown(2);
        }
    }
}

class ReflectionActivity : MindfulnessActivity
{
    private static readonly string[] Prompts =
    {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless."
    };

    public ReflectionActivity() : base("Reflection Activity") { }

    protected override void RunActivity()
    {
        Random rnd = new Random();
        string prompt = Prompts[rnd.Next(Prompts.Length)];
        Console.WriteLine(prompt);
        Thread.Sleep(Duration * 1000);
    }
}

class ListingActivity : MindfulnessActivity
{
    public ListingActivity() : base("Listing Activity") { }

    protected override void RunActivity()
    {
        Console.WriteLine("Start listing items now:");
        int count = 0;
        DateTime endTime = DateTime.Now.AddSeconds(Duration);
        while (DateTime.Now < endTime)
        {
            Console.ReadLine();
            count++;
        }
        Console.WriteLine($"You have listed {count} items.");
    }
}

class Program
{
    private static Dictionary<string, int> activityLog = new Dictionary<string, int>();

    public static void LogActivity(string activityName)
    {
        if (!activityLog.ContainsKey(activityName))
        {
            activityLog[activityName] = 0;
        }
        activityLog[activityName]++;
        Console.WriteLine($"Total times {activityName} performed: {activityLog[activityName]}");
    }

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Menu Options:");
            Console.WriteLine("1. Start breathing activity");
            Console.WriteLine("2. Start reflection activity");
            Console.WriteLine("3. Start listing activity");
            Console.WriteLine("4. Show activity log");
            Console.WriteLine("5. Quit");
            Console.Write("Select a choice from the menu: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var breathingActivity = new BreathingActivity();
                    breathingActivity.StartActivity();
                    break;
                case "2":
                    var reflectionActivity = new ReflectionActivity();
                    reflectionActivity.StartActivity();
                    break;
                case "3":
                    var listingActivity = new ListingActivity();
                    listingActivity.StartActivity();
                    break;
                case "4":
                    foreach (var entry in activityLog)
                    {
                        Console.WriteLine($"{entry.Key}: {entry.Value} times");
                    }
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid selection. Please try again.");
                    break;
            }
        }
    }
}
