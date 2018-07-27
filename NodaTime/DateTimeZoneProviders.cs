// Copyright 2012 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using NodaTime.TimeZones;
using NodaTime.Utility;
using JetBrains.Annotations;

namespace NodaTime
{
    /// <summary>
    /// Static access to date/time zone providers built into Noda Time and for global configuration where this is unavoidable.
    /// All properties are thread-safe, and the providers returned by the read-only properties cache their results.
    /// </summary>
    public class DateTimeZoneProviders
    {
        /// <summary>
        /// Gets a time zone provider which uses a <see cref="TzdbDateTimeZoneSource"/>.
        /// The underlying source is <see cref="TzdbDateTimeZoneSource.Default"/>, which is initialized from
        /// resources within the NodaTime assembly.
        /// </summary>
        /// <value>A time zone provider using a <c>TzdbDateTimeZoneSource</c>.</value>
        [NotNull] public IDateTimeZoneProvider Tzdb
        {
            get
            {
                DateTimeZoneCache TzdbImpl = new DateTimeZoneCache(TzdbDateTimeZoneSource.Default);
                return TzdbImpl;
            }
        }

    }
}
