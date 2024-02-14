using System;
using System.Collections.Generic;

interface IQuestion
{
    string GetQuestion();
    bool CheckAnswer(string answer);
}

abstract class Question : IQuestion
{
    public string Text { get; set; }
    public string Answer { get; set; }

    protected Question(string text, string answer)
    {
        
    }

    public string GetQuestion() => Text; 

    public abstract bool CheckAnswer(string answer);
}


class MultipleChoiceQuestion : Question
{
    public List<string> Options { get; set; }

    public MultipleChoiceQuestion(string text, string answer, List<string> options) : base(text, answer)
    {
        
    }

    public override bool CheckAnswer(string answer)
    {
        
        throw new NotImplementedException();
    }
}

class TrueFalseQuestion : Question
{
    public TrueFalseQuestion(string text, string answer) : base(text, answer)
    {
        
    }

    public override bool CheckAnswer(string answer)
    {
        
        throw new NotImplementedException();
    }
}

class ShortAnswerQuestion : Question
{
    public ShortAnswerQuestion(string text, string answer) : base(text, answer)
    {
        
    }

    public override bool CheckAnswer(string answer)
    {
        
        throw new NotImplementedException();
    }
}

class Quiz
{
    public List<IQuestion> Questions { get; set; } = new List<IQuestion>();

    public void AddQuestion(IQuestion question)
    {
        Questions.Add(question); 
    }
}

class User
{
    public string Name { get; set; }
    public int Score { get; set; } = 0; 
}

class QuizRunner
{
    private Quiz quiz;
    private User user;

    public QuizRunner(Quiz quiz, User user)
    {
        
    }

    public void RunQuiz()
    {
        
        Console.WriteLine("In progress.");
        
    }
}

class FileQuestionLoader
{
    public static Quiz LoadQuizFromFile(string filePath)
    {
        
        throw new NotImplementedException();
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Quiz Program!");
        
        Console.WriteLine("Enter your name:");
        var userName = Console.ReadLine();
        Console.WriteLine($"Hello, {userName}, the quiz is not yet finished, working in progress.");
     
    }
}
