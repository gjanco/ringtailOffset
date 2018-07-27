using Microsoft.SqlServer.Server;
using System;

namespace NodaTime
{
    public class RingtailUtil
    {
        [SqlFunction(DataAccess = DataAccessKind.Read)]
        public static int DaysOff(string date, string time, string rpfTimeZone, string ingestTimeZone)
        {
            int tensPlace = 0;
            DateTime localDt = DateTime.Parse(string.Format("{0} {1}", date, time));
            LocalTime localTime = new LocalTime(localDt.Hour, localDt.Minute, localDt.Second);
            int hour = localDt.Hour;
            LocalDateTime localDateTime = LocalDateTime.FromDateTime(localDt);

            DateTimeZoneProviders dtzp = new DateTimeZoneProviders();

            DateTimeZone rpfZone = dtzp.Tzdb[rpfTimeZone];
            DateTimeZone ingestZone = dtzp.Tzdb[ingestTimeZone];

            ZonedDateTime zonedDateTime = localDateTime.InZoneLeniently(rpfZone);
            Offset rpfOffset = rpfZone.GetUtcOffset(zonedDateTime.ToInstant());
            Offset ingestOffset = ingestZone.GetUtcOffset(zonedDateTime.ToInstant());


            ZonedDateTime zeroTime = new LocalDateTime(LocalDate.FromDateTime(localDt), new LocalTime(0, 15)).InZoneLeniently(ingestZone);
            ZonedDateTime elevenTime = new LocalDateTime(LocalDate.FromDateTime(localDt), new LocalTime(11, 15)).InZoneLeniently(ingestZone);

            ZonedDateTime zeroTimeRpf = new LocalDateTime(LocalDate.FromDateTime(localDt), new LocalTime(0, 15)).InZoneLeniently(rpfZone);
            ZonedDateTime elevenTimeRpf = new LocalDateTime(LocalDate.FromDateTime(localDt), new LocalTime(11, 15)).InZoneLeniently(rpfZone);

            Offset zeroOffset = zeroTimeRpf.Offset - zeroTime.Offset;
            Offset elevenOffset = elevenTimeRpf.Offset - elevenTime.Offset;

            if (Math.Abs(zeroOffset.Milliseconds) < Math.Abs(elevenOffset.Milliseconds))
            {
                var zeroClock = new LocalTime(0, 0);
                var lowTime = zeroClock.PlusMilliseconds(-zeroOffset.Milliseconds);
                var highTime = zeroClock.PlusMilliseconds(-elevenOffset.Milliseconds);
                
                if (localTime >= lowTime && localTime < highTime)
                {
                    tensPlace = 10;
                }

            }

            /*
             * 
             * at0 get offsets and subtract rpfoffat0 - ingestOffsetat0  (-4) 
             * at11 get offsets and subtract rpfoffat11 - ingestoffat11 (-5)
             * 
             * if these are different
             *  range is (0 in time) - at0 or (0 - -4) = 4
             *   to (0 in time) -at11 or (0 - -5) = 5
             *   
             * 
             */

            var offsetDelta = rpfOffset - ingestOffset;
            var hrsToAdd = offsetDelta.Milliseconds / NodaConstants.MillisecondsPerHour;

            var newHour = (hour + hrsToAdd);
            if (newHour < 0)
            {
                return -1-tensPlace;
            }
            else if (newHour > 24)
            {
                return 1+tensPlace;
            }
            else
            {
                return 0+tensPlace;
            }

        }
    }
}
