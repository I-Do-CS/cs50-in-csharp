// https://cs50.harvard.edu/x/2024/psets/1/me/
using System.Globalization;

namespace Hello
{
    class Program
    {
        static void Main()
        {
            Console.Write("What's Your Name? ");
            string? name = Console.ReadLine();

            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Wrong Input, Try Again.");
                Main();
                return;
            
            } 

            TextInfo nameInfo = new CultureInfo("en-US", false).TextInfo;
            name = nameInfo.ToTitleCase(name.Trim().ToLower());

            Console.WriteLine($"Hello, {name}!");
        }
    }
}