using IoT.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCtrl
{
    class ConsoleIntent
    {
        public static ConsoleIntent Exit { get; } = new ConsoleIntent("exit");
        public static ConsoleIntent On { get; } = new ConsoleIntent("on");
        public static ConsoleIntent Off { get; } = new ConsoleIntent("off");
        public static ConsoleIntent Red { get; } = new ConsoleIntent("rot");
        public static ConsoleIntent Green { get; } = new ConsoleIntent("grün");
        public static ConsoleIntent Blue { get; } = new ConsoleIntent("blau");
        public static ConsoleIntent White { get; } = new ConsoleIntent("weiß");
        public static ConsoleIntent RandomPattern { get; } = new ConsoleIntent("Zufall");
        public static ConsoleIntent RedAlert { get; } = new ConsoleIntent("RedAlert");

        public static IReadOnlyCollection<ConsoleIntent> AllIntents { get; } = new ConsoleIntent[]
        {
            Exit, On, Off, Red, Green, Blue, White, RandomPattern, RedAlert
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
            else if(this == On)
            {
                IoTcoreLedService.Voltage_On();
                IoTcoreLedService.SolidColor(Color.White);
            }
            else if (this == Off)
            {
                IoTcoreLedService.SolidColor(Color.Black);
                IoTcoreLedService.Voltage_Off();
            }
            else if (this == Red)
            {
                IoTcoreLedService.SolidColor(Color.Red);
            }
            else if (this == Green)
            {
                IoTcoreLedService.SolidColor(Color.Green);
            }
            else if (this == Blue)
            {
                IoTcoreLedService.SolidColor(Color.Blue);
            }
            else if (this == White)
            {
                IoTcoreLedService.SolidColor(Color.Blue);
            }
            else if (this == RandomPattern)
            {
                IoTcoreLedService.AnimateRandom();
            }
            else if (this == RedAlert)
            {
                int spinCount = 10;
                IoTcoreLedService.Spin(Color.Red, spinCount);
            }
        }


        public string Keyword { get; set; }


        public ConsoleIntent(string keyword)
        {
            this.Keyword = keyword;
        }
    }
}
