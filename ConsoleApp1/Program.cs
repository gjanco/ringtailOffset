using NodaTime;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            int offset = RingtailUtil.DaysOff("10/28/2001", "01:15:00", "America/Los_Angeles", "America/New_York");
        }
    }
}
