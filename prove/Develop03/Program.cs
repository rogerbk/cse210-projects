using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        var scriptureLibrary = new ScriptureLibrary();
        scriptureLibrary.AddScripture(new Scripture(new ScriptureReference("Proverbs", 3, 5, 6), "Trust in the LORD with all thine heart; and lean not unto thine own understanding. In all thy ways acknowledge him, and he shall direct thy paths."));

        scriptureLibrary.AddScripture(new Scripture(new ScriptureReference("Psalm", 23, 1), "The LORD is my shepherd; I shall not want."));

        scriptureLibrary.AddScripture(new Scripture(new ScriptureReference("Matthew", 5, 9), "Blessed are the peacemakers: for they shall be called the children of God."));

        scriptureLibrary.AddScripture(new Scripture(new ScriptureReference("Isaiah", 40, 31), "But they that wait upon the LORD shall renew their strength; they shall mount up with wings as eagles; they shall run, and not be weary; and they shall walk, and not faint."));
        // Add more scriptures here

        Random random = new Random();

        string input;
        do
        {
            var scripture = scriptureLibrary.GetRandomScripture(random);
            Console.Clear();
            Console.WriteLine(scripture.FullText);

            Console.WriteLine("Press enter to continue or type 'quit' to finish:");
            input = Console.ReadLine();

            if (input.ToLower() != "quit")
            {
                scripture.HideRandomWords();
                Console.Clear();
                Console.WriteLine(scripture.FullText);
            }

        } while (input.ToLower() != "quit");

        Console.WriteLine("Program will now exit.");
    }
}

class ScriptureLibrary
{
    private List<Scripture> Scriptures = new List<Scripture>();

    public void AddScripture(Scripture scripture)
    {
        Scriptures.Add(scripture);
    }

    public Scripture GetRandomScripture(Random random)
    {
        int index = random.Next(Scriptures.Count);
        return Scriptures[index];
    }
}

class Scripture
{
    public ScriptureReference Reference { get; private set; }
    private List<Word> Words;
    public bool AreAllWordsHidden => Words.All(w => w.IsHidden);

    public Scripture(ScriptureReference reference, string text)
    {
        Reference = reference;
        Words = text.Split(' ').Select(word => new Word(word)).ToList();
    }

    public void HideRandomWords()
    {
        var rnd = new Random();
        foreach (var word in Words.Where(w => !w.IsHidden).OrderBy(x => rnd.Next()))
        {
            word.Hide();
            break;
        }
    }

    public string FullText
    {
        get
        {
            var text = string.Join(" ", Words.Select(w => w.Text));
            return $"{Reference} {text}";
        }
    }
}

class ScriptureReference
{
    public string Book { get; private set; }
    public int Chapter { get; private set; }
    public int StartVerse { get; private set; }
    public int? EndVerse { get; private set; }

    public ScriptureReference(string book, int chapter, int startVerse, int? endVerse = null)
    {
        Book = book;
        Chapter = chapter;
        StartVerse = startVerse;
        EndVerse = endVerse;
    }

    public override string ToString()
    {
        return $"{Book} {Chapter}:{StartVerse}" + (EndVerse != null ? $"-{EndVerse}" : "");
    }
}

class Word
{
    private string _word;
    public bool IsHidden { get; private set; }

    public Word(string word)
    {
        _word = word;
    }

    public void Hide()
    {
        IsHidden = true;
    }

    public string Text => IsHidden ? "____" : _word;
}

//As part of the exceeding challege I added this: Have your program work with a library of scriptures rather than a single one. Choose scriptures at random to present to the user. In this example, I've added three additional scriptures (Psalm 23:1, Matthew 5:9, and Isaiah 40:31) to the scripture library.