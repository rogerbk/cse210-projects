// I have added this: 
// Exception Handling, Input Validation and Tracking negative goals with a point penalty system by creating
// the NegativeGoal Class

using System;
using System.Collections.Generic;
using System.IO;

abstract class Goal
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Points { get; set; }

    public abstract void RecordEvent(ref int userScore);

    protected Goal()
    {
        Name = "";
        Description = "";
        Points = 0;
    }
}

class SimpleGoal : Goal
{
    public bool IsCompleted { get; private set; }

    public SimpleGoal(bool isCompleted = false)
    {
        IsCompleted = isCompleted;
    }

    public override void RecordEvent(ref int userScore)
    {
        if (!IsCompleted)
        {
            IsCompleted = true;
            userScore += Points;
            Console.WriteLine($"Completed goal: {Name}, earned points: {Points}. Total score: {userScore}");
        }
        else
        {
            Console.WriteLine($"Goal: {Name} is already completed.");
        }
    }
}

class EternalGoal : Goal
{
    public override void RecordEvent(ref int userScore)
    {
        userScore += Points;
        Console.WriteLine($"Recorded event for eternal goal: {Name}, earned points: {Points}. Total score: {userScore}");
    }
}

class ChecklistGoal : Goal
{
    public int TotalTimes { get; set; }
    private int completedTimes;
    public int CompletedTimes
    {
        get { return completedTimes; }
        private set { completedTimes = value; } 
    }

    public ChecklistGoal(int totalTimes = 0, int completedTimes = 0)
    {
        TotalTimes = totalTimes;
        this.completedTimes = completedTimes;
    }

    public override void RecordEvent(ref int userScore)
    {
        if (CompletedTimes < TotalTimes)
        {
            CompletedTimes++;
            userScore += Points;
            if (CompletedTimes == TotalTimes)
            {
                userScore += 500; //Bonus
                Console.WriteLine($"Completed checklist goal: {Name}, earned bonus points: 500. Total score: {userScore}");
            }
            else
            {
                Console.WriteLine($"Recorded event for checklist goal: {Name}, progress: {CompletedTimes}/{TotalTimes}, earned points: {Points}. Total score: {userScore}");
            }
        }
        else
        {
            Console.WriteLine($"Checklist goal: {Name} already completed.");
        }
    }

    public void IncrementCompletedTimes()
    {
        if (CompletedTimes < TotalTimes)
        {
            CompletedTimes++;
        }
    }
}

class NegativeGoal : Goal
{
    public override void RecordEvent(ref int userScore)
    {
        userScore -= Points;
        Console.WriteLine($"Recorded event for negative goal: {Name}, lost points: {Points}. Total score: {userScore}");
    }
}

class Program
{
    static List<Goal> goals = new List<Goal>();
    static int userScore = 0;
    static readonly string fileName = "goals.txt";

    static void Main()
    {
        LoadGoals();

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\nWelcome to the Eternal Quest program!");
            Console.WriteLine("1. Create a new goal");
            Console.WriteLine("2. List all goals");
            Console.WriteLine("3. Record event");
            Console.WriteLine("4. Save and exit");
            Console.WriteLine("5. Exit without saving");

            if (!int.TryParse(Console.ReadLine(), out int option))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            switch (option)
            {
                case 1:
                    CreateGoal();
                    break;
                case 2:
                    ListGoals();
                    break;
                case 3:
                    RecordEvent();
                    break;
                case 4:
                    SaveGoals();
                    exit = true;
                    break;
                case 5:
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }

        Console.WriteLine("Thank you for using the Eternal Quest program. Goodbye!");
    }

    static void CreateGoal()
    {
        Console.WriteLine("\nPlease enter the name of the goal:");
        string name = Console.ReadLine();
        Console.WriteLine("Please enter the description of the goal:");
        string description = Console.ReadLine();
        Console.WriteLine("Please enter the type of the goal (simple, eternal, checklist, negative):");
        string type = Console.ReadLine().ToLower();
        Console.WriteLine("Please enter the points for the goal (positive for gaining points, negative for losing points):");
        int points = int.TryParse(Console.ReadLine(), out int result) ? result : 0;

        Goal goal = type switch
        {
            "simple" => new SimpleGoal { Name = name, Description = description, Points = points },
            "eternal" => new EternalGoal { Name = name, Description = description, Points = points },
            "checklist" => new ChecklistGoal { Name = name, Description = description, Points = points, TotalTimes = AskForTotalTimes() },
            "negative" => new NegativeGoal { Name = name, Description = description, Points = points },
            _ => null
        };

        if (goal != null)
        {
            goals.Add(goal);
            Console.WriteLine("Goal created successfully.");
        }
        else
        {
            Console.WriteLine("Invalid goal type.");
        }
    }

    static int AskForTotalTimes()
    {
        Console.WriteLine("Please enter the total times for the checklist goal:");
        return int.TryParse(Console.ReadLine(), out int totalTimes) ? totalTimes : 0;
    }

    static void ListGoals()
    {
        Console.WriteLine("\nHere are your goals:");
        foreach (var goal in goals)
        {
            string completionStatus = goal switch
            {
                SimpleGoal sg => sg.IsCompleted ? "[X]" : "[ ]",
                ChecklistGoal cg => $"[Completed {cg.CompletedTimes}/{cg.TotalTimes}]",
                _ => ""
            };
            Console.WriteLine($"Name: {goal.Name} {completionStatus}");
            Console.WriteLine($"Description: {goal.Description}");
            Console.WriteLine($"Points: {goal.Points}\n");
        }
        Console.WriteLine($"Your current score: {userScore}");
    }

    static void RecordEvent()
    {
        Console.WriteLine("\nPlease enter the name of the goal you want to record an event for:");
        string name = Console.ReadLine();
        var goal = goals.Find(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (goal != null)
        {
            goal.RecordEvent(ref userScore);
        }
        else
        {
            Console.WriteLine("Goal not found. Please try again.");
        }
    }

    static void SaveGoals()
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            writer.WriteLine(userScore);
            foreach (var goal in goals)
            {
                writer.WriteLine(goal.Name);
                writer.WriteLine(goal.Description);
                writer.WriteLine(goal.Points);
                writer.WriteLine(goal.GetType().Name);
                if (goal is ChecklistGoal cg)
                {
                    writer.WriteLine(cg.TotalTimes);
                    writer.WriteLine(cg.CompletedTimes);
                }
                else if (goal is SimpleGoal sg)
                {
                    writer.WriteLine(sg.IsCompleted);
                }
            }
        }
        Console.WriteLine("Goals and score saved successfully.");
    }

     static void LoadGoals()
    {
        if (File.Exists(fileName))
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                if (int.TryParse(reader.ReadLine(), out int score))
                {
                    userScore = score;
                }

                while (!reader.EndOfStream)
                {
                    string name = reader.ReadLine();
                    string description = reader.ReadLine();
                    int points = int.TryParse(reader.ReadLine(), out int result) ? result : 0;
                    string type = reader.ReadLine();

                    Goal goal = null;
                    switch (type)
                    {
                        case "SimpleGoal":
                            goal = new SimpleGoal(bool.Parse(reader.ReadLine())) { Name = name, Description = description, Points = points };
                            break;
                        case "EternalGoal":
                            goal = new EternalGoal { Name = name, Description = description, Points = points };
                            break;
                        case "ChecklistGoal":
                            int totalTimes = int.Parse(reader.ReadLine());
                            int completedTimes = int.Parse(reader.ReadLine());
                            goal = new ChecklistGoal(totalTimes, completedTimes) { Name = name, Description = description, Points = points };
                            break;
                        case "NegativeGoal":
                            goal = new NegativeGoal { Name = name, Description = description, Points = points };
                            break;
                    }

                    if (goal != null)
                    {
                        goals.Add(goal);
                    }
                }
            }
            Console.WriteLine("Goals and score loaded successfully.");
        }
    }
} 