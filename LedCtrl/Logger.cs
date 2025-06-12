using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCtrl
{
    class Logger
    {
        private Logger() { }
        public static Logger Instance { get; } = new Logger();

        internal void LogException(string v, Exception ex)
        {
            Console.WriteLine("[exception]\t" + v + ": " + ex.Message);
        }

        internal void LogDebug(string v)
        {
            Console.WriteLine("[debug]\t" + v);
        }
    }
}
