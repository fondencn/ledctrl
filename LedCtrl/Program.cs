using System;
using System.Linq;

namespace LedCtrl
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("*************************************************");
            Console.WriteLine("* Respeaker PI APA102 LED / USB Controller Demo *");
            Console.WriteLine("*************************************************");
            Console.WriteLine("");
            Console.WriteLine("Valid options are: " + String.Join(" , ", ConsoleIntent.AllIntents.Select(item => item.Keyword)));
            Console.WriteLine("");
            Console.WriteLine("Select usb or hat to use the respective device.");
            Console.WriteLine("**************************************************");
            Console.WriteLine("Enter a command to start or type 'exit' to quit.");
            int returnCode = 0;
            try
            {
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
                            Console.WriteLine("Handle command: " + intent.Keyword + "... ");
                            intent.Handle();
                        }
                        else
                        {
                            Console.WriteLine("no matching command found - please enter valid command");
                        }
                    }
                    while (intent != ConsoleIntent.Exit);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                returnCode = 1;
            }
            finally
            {
                ConsoleIntent.Dispose();
            }
            return returnCode;
        }
    }
}
