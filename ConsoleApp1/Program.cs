using NodaTime;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            int offset = RingtailUtil.DaysOff("10/28/2001", "04:15:00", "America/New_York", "UTC");
        }
    }
}
