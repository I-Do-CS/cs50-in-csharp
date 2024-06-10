// https://cs50.harvard.edu/x/2024/psets/2/readability/
using LanguageDetection;

DisplayAppInfo();

string textMaterial = GetEnglishText();

Dictionary<string, int> textInfo = GetTextInfo(textMaterial);

int readingLevel = GetReadingLevel(textInfo);

string textGrade = readingLevel switch
{
    < 1 => "Before Grade 1",
    > 16 => "Grade 16+",
    _ => $"Grade {readingLevel}"
};

Console.WriteLine(textGrade);
return;


static int GetReadingLevel(Dictionary<string, int> textInfo)
{
    // Use Coleman-Liau's Index to calculate the reading level
    double L = (double)textInfo["letters"] / textInfo["words"] * 100;
    double S = (double)textInfo["sentences"] / textInfo["words"] * 100;

    double index = (0.0588 * L) - (0.296 * S) - 15.8;

    return (int)Math.Round(index);
}

static Dictionary<string, int> GetTextInfo(string text)
{
    /// Calculates and returns the number of sentences, words and 
    /// letters of a text as a Dictionary.
    Dictionary<string, int> textInfo = new()
    {
        {"letters", 0 },
        {"words", 0 },
        {"sentences", 0 },
    };

    foreach (char c in text)
    {
        if (char.IsAsciiLetter(c))
        {
            textInfo["letters"]++;
        }
        else if (c.Equals('.') || c.Equals('!') || c.Equals('?'))
        {
            textInfo["sentences"]++;
        }
    }

    // Split words on whitespace chars and get rid of extra whitspaces
    string[] wordsArr = text.Split(" ");
    List<string> actualWords = [];
    for (int i = 0; i < wordsArr.Length; i++)
    {
        if (wordsArr[i].Equals(" "))
        {
            continue;
        }
        else
        {
            actualWords.Add(wordsArr[i]);
        }
    }

    textInfo["words"] = actualWords.Count;

    return textInfo;
}

static string GetEnglishText()
{
    Console.Write("Enter Your English Text: ");
    string? textMaterial = Console.ReadLine();

    // Default error message
    string errorMessage = "Somthing went wrong!";

    try
    {
        if (string.IsNullOrEmpty(textMaterial))
        {
            errorMessage = "No text was given, try again.";
            throw new Exception();
        }

        if (textMaterial.Length < 30) 
        {
            errorMessage = "Your text is too short, try again.";
            throw new Exception();
        }

        LanguageDetector detector = new();
        detector.AddAllLanguages();
        if (!detector.Detect(textMaterial).Equals("eng"))
        {
            errorMessage = "Text is not in English, try again.";
            throw new Exception();
        }
    }
    catch (Exception)
    {
        Console.WriteLine(errorMessage + "\n");
        return GetEnglishText();
    }

    return textMaterial.Trim();
}

static void DisplayAppInfo()
{
    const int LINE_LENGTH = 70;
    Console.Title = "Readability - CS50X";
    Console.ForegroundColor = ConsoleColor.Green;

    // Print horizontal line 
    for (int i = 0; i < LINE_LENGTH; i++)
    {
        Console.Write("=");
    }
    Console.WriteLine(); // Newline

    Console.WriteLine(
        "TITLE: Readability\n\n" +
        "DESCRIPTION: A console app that calculates the reading\n" +
        "    -level of English text material based on the Coleman-Liau's index.\n\n" +
        "AUTHOR: Ali Ghelichkhani");

    // Print horizontal line 
    for (int i = 0; i < LINE_LENGTH; i++)
    {
        Console.Write("=");
    }
    Console.WriteLine(); // Newline


    Console.ResetColor();
}