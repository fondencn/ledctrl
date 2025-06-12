using System;
using System.Linq;

namespace LedCtrl
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*******************************************");
            Console.WriteLine("* Raspberry PI APA102 LED Controller Demo *");
            Console.WriteLine("*******************************************");
            Console.WriteLine("Valid options are: " + String.Join(" , ", ConsoleIntent.AllIntents.Select(item => item.Keyword)));

            ConsoleIntent intent;
            if (args?.Any() == true)
            {
                foreach (string arg in args)
                {
                    intent = ConsoleIntent.Parse(arg);
                    intent.Handle();
                }
            }
            else
            {
                do
                {
                    string consoleInput = Console.ReadLine();
                    intent = ConsoleIntent.Parse(consoleInput);
                    if (intent != null)
                    {
                        Console.WriteLine("Handle Intent: " + intent.Keyword + "...");
                        intent.Handle();
                    }
                    else
                    {
                        Console.WriteLine("no matching intent found - did you enter something stupid?");
                    }
                }
                while (intent != ConsoleIntent.Exit);
            }
        }
    }
}
