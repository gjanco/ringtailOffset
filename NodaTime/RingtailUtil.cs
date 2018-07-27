using Microsoft.SqlServer.Server;
using System;

namespace NodaTime
{
    public class RingtailUtil
    {
        [SqlFunction(DataAccess = DataAccessKind.Read)]
        public static int DaysOff(string date, string time, string rpfTimeZone, string ingestTimeZone)
        {
            DateTime localDt = DateTime.Parse(string.Format("{0} {1}", date, time));
            int hour = localDt.Hour;
            LocalDateTime localDateTime = LocalDateTime.FromDateTime(localDt);

            DateTimeZoneProviders dtzp = new DateTimeZoneProviders();

            DateTimeZone rpfZone = dtzp.Tzdb[rpfTimeZone];
            DateTimeZone ingestZone = dtzp.Tzdb[ingestTimeZone];

            ZonedDateTime zonedDateTime = localDateTime.InZoneLeniently(rpfZone);



            Offset rpfOffset = rpfZone.GetUtcOffset(zonedDateTime.ToInstant());
            Offset ingestOffset = ingestZone.GetUtcOffset(zonedDateTime.ToInstant());

            var offsetDelta = rpfOffset - ingestOffset;
            var hrsToAdd = offsetDelta.Milliseconds / NodaConstants.MillisecondsPerHour;

            var newHour = (hour + hrsToAdd);
            if (newHour < 0)
            {
                return -1;
            }
            else if (newHour > 24)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }
    }
}
