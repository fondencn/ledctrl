using IoT.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCtrl
{
    internal class ConsoleIntent
    {
        private static ILEDService _ledService;



        public static ConsoleIntent Exit { get; } = new ConsoleIntent("exit");
        public static ConsoleIntent Usb { get; } = new ConsoleIntent("usb");
        public static ConsoleIntent Hat { get; } = new ConsoleIntent("hat");
        public static ConsoleIntent Red { get; } = new ConsoleIntent("rot");
        public static ConsoleIntent Green { get; } = new ConsoleIntent("grün");
        public static ConsoleIntent Blue { get; } = new ConsoleIntent("blau");
        public static ConsoleIntent White { get; } = new ConsoleIntent("weiß");
        public static ConsoleIntent Black { get; } = new ConsoleIntent("aus");
        public static ConsoleIntent Wait { get; } = new ConsoleIntent("wait");
        public static ConsoleIntent Spin { get; } = new ConsoleIntent("spin");
        public static ConsoleIntent Trace { get; } = new ConsoleIntent("trace");
        public static ConsoleIntent Listen { get; } = new ConsoleIntent("listen");
        public static ConsoleIntent Speak { get; } = new ConsoleIntent("speak");

       public static ConsoleIntent RedAlert { get; } = new ConsoleIntent("RedAlert");

        public static IReadOnlyCollection<ConsoleIntent> AllIntents { get; } = new ConsoleIntent[]
        {
            Exit, Usb, Hat, Red, Green, Blue, White, RedAlert, 
            Black, Wait, Spin, Trace, Listen, Speak
        }.ToList()
         .AsReadOnly();


        internal static ConsoleIntent Parse(string consoleInput)
        {
            return AllIntents.FirstOrDefault(item => String.Equals(item.Keyword, consoleInput, StringComparison.OrdinalIgnoreCase));
        }


        internal void Handle()
        {
            if (this == Exit)
            {
                Console.WriteLine("exiting...");
                
            }

            else if (this == Usb)
            {
                _ledService = new RespeakerUsbDevice();
            }

            else if (this == Hat)
            {
                _ledService = new RespeakerHatDevice();
            }

            else if (this == Red)
            {
                _ledService.SetSolidColor(Color.Red);
            }
            else if (this == Green)
            {
                _ledService.SetSolidColor(Color.Green);
            }
            else if (this == Blue)
            {
                _ledService.SetSolidColor(Color.Blue);
            }
            else if (this == White)
            {
                _ledService.SetSolidColor(Color.White);
            }
            else if (this == Black)
            {
                _ledService.SetSolidColor(Color.Black);
            }
            else if (this == Wait)
            {
                _ledService.Wait();
            }
            else if (this == Spin)
            {
                _ledService.Spin(Color.White, 5);
            }
            else if (this == Trace)
            {
                _ledService.Trace();
            }
            else if (this == Listen)
            {
                _ledService.Listen();
            }
            else if (this == Speak)
            {
                _ledService.Speak();
            }
            // Special case for RedAlert
            else if (this == RedAlert)
            {
                int spinCount = 10;
                _ledService.Spin(Color.Red, spinCount);
            }
        }

        internal static void Dispose()
        {
            (_ledService as IDisposable)?.Dispose();
        }

        public string Keyword { get; set; }


        public ConsoleIntent(string keyword)
        {
            this.Keyword = keyword;
        }
    }
}
