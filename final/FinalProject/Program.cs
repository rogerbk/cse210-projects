using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        Text = text;
        Answer = answer;
    }

    public string GetQuestion() => Text;

    public abstract bool CheckAnswer(string answer);
}


class MultipleChoiceQuestion : Question
{
    public List<string> Options { get; set; }

    public MultipleChoiceQuestion(string text, string answer, List<string> options) : base(text, answer)
    {
        Options = options;
    }

    public override bool CheckAnswer(string answer)
    {
        return answer.Trim().ToLower() == Answer.Trim().ToLower();
    }
}

class TrueFalseQuestion : Question
{
    public TrueFalseQuestion(string text, string answer) : base(text, answer) { }

    public override bool CheckAnswer(string answer)
    {
        return answer.Trim().ToLower().Equals(Answer.Trim().ToLower(), StringComparison.OrdinalIgnoreCase);
    }
}

class ShortAnswerQuestion : Question
{
    public ShortAnswerQuestion(string text, string answer) : base(text, answer) { }

    public override bool CheckAnswer(string answer)
    {
        return answer.Trim().ToLower() == Answer.Trim().ToLower();
    }
}

class Quiz
{
    public List<IQuestion> Questions { get; set; } = new List<IQuestion>();
    public void AddQuestion(IQuestion question) => Questions.Add(question);
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
        this.quiz = quiz;
        this.user = user;
    }

    public void RunQuiz()
{
    Console.WriteLine($"Welcome, {user.Name}! Let's start the quiz.\n");

    foreach (var question in quiz.Questions)
    {
        Console.WriteLine(question.GetQuestion());
        if (question is MultipleChoiceQuestion mcq)
        {
            Console.WriteLine("Options:");
            mcq.Options.ForEach(o => Console.WriteLine($"- {o}"));
        }
        var answer = Console.ReadLine();
        if (question.CheckAnswer(answer))
        {
            Console.WriteLine("Correct!\n");
            user.Score++;
        }
        else
        {
            
            if (question is Question q)
            {
                Console.WriteLine($"Wrong answer. The correct answer is: {q.Answer}\n");
            }
            else
            {
                Console.WriteLine("Wrong answer.\n");
            }
        }
    }

    Console.WriteLine($"Quiz completed. {user.Name}, your final score is {user.Score}/{quiz.Questions.Count}.");
    }
}

class FileQuestionLoader
{
    public static Quiz LoadQuizFromFile(string filePath)
    {
        var quiz = new Quiz();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var parts = line.Split('|');
            if (parts.Length < 3) continue;

            var questionType = parts[0];
            var questionText = parts[1];
            var answer = parts[2];
            var options = parts.Length > 3 ? parts[3].Split(';').ToList() : null;

            switch (questionType)
            {
                case "MC":
                    quiz.AddQuestion(new MultipleChoiceQuestion(questionText, answer, options));
                    break;
                case "TF":
                    quiz.AddQuestion(new TrueFalseQuestion(questionText, answer));
                    break;
                case "SA":
                    quiz.AddQuestion(new ShortAnswerQuestion(questionText, answer));
                    break;
            }
        }

        return quiz;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Enter your name: ");
        var userName = Console.ReadLine();
        var user = new User { Name = userName };

        var quiz = FileQuestionLoader.LoadQuizFromFile("questions.txt");
        var runner = new QuizRunner(quiz, user);

        runner.RunQuiz();
    }
}
