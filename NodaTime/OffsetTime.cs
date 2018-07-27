// Copyright 2017 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using JetBrains.Annotations;
using NodaTime.Annotations;
using NodaTime.Text;
using NodaTime.Utility;
using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NodaTime
{
    /// <summary>
    /// A combination of a <see cref="LocalTime"/> and an <see cref="Offset"/>, to represent
    /// a time-of-day at a specific offset from UTC but without any date information.
    /// </summary>
    /// <threadsafety>This type is an immutable value type. See the thread safety section of the user guide for more information.</threadsafety>

    public struct OffsetTime : IEquatable<OffsetTime>
    {
        [ReadWriteForEfficiency] private LocalTime time;
        [ReadWriteForEfficiency] private Offset offset;

        /// <summary>
        /// Constructs an instance of the specified time and offset.
        /// </summary>
        /// <param name="time">The time part of the value.</param>
        /// <param name="offset">The offset part of the value.</param>
        public OffsetTime(LocalTime time, Offset offset)
        {
            this.time = time;
            this.offset = offset;
        }

        /// <summary>
        /// Gets the time-of-day represented by this value.
        /// </summary>
        /// <value>The time-of-day represented by this value.</value>
        public LocalTime TimeOfDay => time;

        /// <summary>
        /// Gets the offset from UTC of this value.
        /// <value>The offset from UTC of this value.</value>
        /// </summary>
        public Offset Offset => offset;

        /// <summary>
        /// Gets the hour of day of this offset time, in the range 0 to 23 inclusive.
        /// </summary>
        /// <value>The hour of day of this offset time, in the range 0 to 23 inclusive.</value>
        public int Hour => time.Hour;

        /// <summary>
        /// Gets the hour of the half-day of this offset time, in the range 1 to 12 inclusive.
        /// </summary>
        /// <value>The hour of the half-day of this offset time, in the range 1 to 12 inclusive.</value>
        public int ClockHourOfHalfDay => time.ClockHourOfHalfDay;

        // TODO(feature): Consider exposing this.
        /// <summary>
        /// Gets the hour of the half-day of this offset time, in the range 0 to 11 inclusive.
        /// </summary>
        /// <value>The hour of the half-day of this offset time, in the range 0 to 11 inclusive.</value>
        internal int HourOfHalfDay => time.HourOfHalfDay;

        /// <summary>
        /// Gets the minute of this offset time, in the range 0 to 59 inclusive.
        /// </summary>
        /// <value>The minute of this offset time, in the range 0 to 59 inclusive.</value>
        public int Minute => time.Minute;

        /// <summary>
        /// Gets the second of this offset time within the minute, in the range 0 to 59 inclusive.
        /// </summary>
        /// <value>The second of this offset time within the minute, in the range 0 to 59 inclusive.</value>
        public int Second => time.Second;

        /// <summary>
        /// Gets the millisecond of this offset time within the second, in the range 0 to 999 inclusive.
        /// </summary>
        /// <value>The millisecond of this offset time within the second, in the range 0 to 999 inclusive.</value>
        public int Millisecond => time.Millisecond;

        /// <summary>
        /// Gets the tick of this offset time within the second, in the range 0 to 9,999,999 inclusive.
        /// </summary>
        /// <value>The tick of this offset time within the second, in the range 0 to 9,999,999 inclusive.</value>
        public int TickOfSecond => time.TickOfSecond;

        /// <summary>
        /// Gets the tick of this offset time within the day, in the range 0 to 863,999,999,999 inclusive.
        /// </summary>
        /// <remarks>
        /// If the value does not fall on a tick boundary, it will be truncated towards zero.
        /// </remarks>
        /// <value>The tick of this offset time within the day, in the range 0 to 863,999,999,999 inclusive.</value>
        public long TickOfDay => time.TickOfDay;

        /// <summary>
        /// Gets the nanosecond of this offset time within the second, in the range 0 to 999,999,999 inclusive.
        /// </summary>
        /// <value>The nanosecond of this offset time within the second, in the range 0 to 999,999,999 inclusive.</value>
        public int NanosecondOfSecond => time.NanosecondOfSecond;

        /// <summary>
        /// Gets the nanosecond of this offset time within the day, in the range 0 to 86,399,999,999,999 inclusive.
        /// </summary>
        /// <value>The nanosecond of this offset time within the day, in the range 0 to 86,399,999,999,999 inclusive.</value>
        public long NanosecondOfDay => time.NanosecondOfDay;

        /// <summary>
        /// Creates a new <see cref="OffsetTime"/> for the same time-of-day, but with the specified UTC offset.
        /// </summary>
        /// <param name="offset">The new UTC offset.</param>
        /// <returns>A new <c>OffsetTime</c> for the same date, but with the specified UTC offset.</returns>
        [Pure]
        public OffsetTime WithOffset(Offset offset) => new OffsetTime(this.time, offset);

        
        /// <summary>
        /// Combines this <see cref="OffsetTime"/> with the given <see cref="LocalDate"/>
        /// into an <see cref="OffsetDateTime"/>.
        /// </summary>
        /// <param name="date">The date to combine with this time-of-day.</param>
        /// <returns>The <see cref="OffsetDateTime"/> representation of this time-of-day on the given date.</returns>
        [Pure]
        public OffsetDateTime On(LocalDate date) => new OffsetDateTime(date.At(time), Offset);

        /// <summary>
        /// Returns a hash code for this offset time.
        /// </summary>
        /// <returns>A hash code for this offset time.</returns>
        public override int GetHashCode() => HashCodeHelper.Hash(TimeOfDay, Offset);

        /// <summary>
        /// Compares two <see cref="OffsetTime"/> values for equality. This requires
        /// that the time-of-day values be the same and the offsets.
        /// </summary>
        /// <param name="obj">The object to compare this offset time with.</param>
        /// <returns>True if the given value is another offset time equal to this one; false otherwise.</returns>
        public override bool Equals(object obj) => obj is OffsetTime other && Equals(other);

        /// <summary>
        /// Compares two <see cref="OffsetTime"/> values for equality. This requires
        /// that the date values be the same and the offsets.
        /// </summary>
        /// <param name="other">The value to compare this offset time with.</param>
        /// <returns>True if the given value is another offset time equal to this one; false otherwise.</returns>
        public bool Equals(OffsetTime other) => TimeOfDay == other.TimeOfDay && Offset == other.Offset;

        /// <summary>
        /// Implements the operator == (equality).
        /// </summary>
        /// <param name="left">The left hand side of the operator.</param>
        /// <param name="right">The right hand side of the operator.</param>
        /// <returns><c>true</c> if values are equal to each other, otherwise <c>false</c>.</returns>
        public static bool operator ==(OffsetTime left, OffsetTime right) => left.Equals(right);

        /// <summary>
        /// Implements the operator != (inequality).
        /// </summary>
        /// <param name="left">The left hand side of the operator.</param>
        /// <param name="right">The right hand side of the operator.</param>
        /// <returns><c>true</c> if values are not equal to each other, otherwise <c>false</c>.</returns>
        public static bool operator !=(OffsetTime left, OffsetTime right) => !(left == right);
        
        ///<summary>
        /// Deconstruct this value into its components.
        /// </summary>
        /// <param name="localTime">
        /// The <see cref="LocalTime"/> component.
        /// </param>
        /// <param name="offset">
        /// The <see cref="Offset"/> component.
        /// </param>
        [Pure]
        public void Deconstruct(out LocalTime localTime, out Offset offset)
        {
            localTime = TimeOfDay;
            offset = Offset;
        }
    }
}
