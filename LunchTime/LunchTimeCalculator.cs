using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchTime
{
    public class LunchTimeCalculator
    {
        /// <summary>
        /// Defines the starting time for the lunch period. Only the hours
        /// and minutes fields are considered.
        /// </summary>
        public DateTime LunchStart = DateTime.Parse("13:00");

        /// <summary>
        /// Defines the ending time for the lunch period. Only the hours
        /// and minutes fields are considered.
        /// </summary>
        public DateTime LunchEnd = DateTime.Parse("14:00");

        /// <summary>
        /// Defines which days are to be considered weekdays. Days of the week
        /// that have their values set to false will not be counted when
        /// tallying up lunch hours.
        /// </summary>
        private Dictionary<DayOfWeek, bool> weekdays = new Dictionary<DayOfWeek, bool>()
        {
            { DayOfWeek.Monday, true },
            { DayOfWeek.Tuesday, true },
            { DayOfWeek.Wednesday, true },
            { DayOfWeek.Thursday, true },
            { DayOfWeek.Friday, true },
            { DayOfWeek.Saturday, false },
            { DayOfWeek.Sunday, false }
        };

        /// <summary>
        /// List of specific dates which are not to be counted when tallying up lunch hours.
        /// Time values for the items in this list are ignored (only Date values are counted).
        /// </summary>
        public List<DateTime> Holidays = new List<DateTime>();

        /// <summary>
        /// Given a starting date and an ending date, this method calculates how much time in between
        /// those dates was spent on lunch breaks. Weekends and holidays are not counted.
        /// </summary>
        /// <example>
        /// LunchTimeCalculator ltc = new LunchTimeCalculator();
        /// // By default, lunch starts at 13:00 and ends at 14:00
        /// DateTime start = DateTime.Parse("2018-07-09 10:00");
        /// DateTime end = DateTime.Parse("2018-07-09 18:30");
        /// TimeSpan lunches = ltc(start, end);
        /// // lunches.TotalMinutes = 60;
        /// </example>
        /// <example>
        /// LunchTimeCalculator ltc = new LunchTimeCalculator();
        /// ltc.LunchStart = DateTime.Parse("12:30");
        /// ltc.LunchEnd = DateTime.Parse("13:00");
        /// 
        /// DateTime start = DateTime.Parse("2018-07-09 10:00");
        /// DateTime end = DateTime.Parse("2018-07-10 12:45");
        /// TimeSpan lunches = ltc(start, end);
        /// // 30 hours on the 9th, + 15 minutes on the 10th
        /// // lunches.TotalMinutes = 45;
        /// </example>
        /// <param name="Start">Date/time at which to start counting.</param>
        /// <param name="End">Date/time at which to finish counting.</param>
        /// <returns>Amount of hours dedicated to lunch between the specified
        /// dates, excluding weekends and holidays.</returns>
        public TimeSpan LunchHoursInSpan(DateTime Start, DateTime End)
        {
            if (End < Start)
                throw new ArgumentException("Start date must preceed End date");

            // first day is always calculated
            TimeSpan first = FirstDay(Start, End);

            // If there are any days in between the first and last day
            TimeSpan middle = new TimeSpan();
            DateTime secondDay = Start.AddDays(1);
            DateTime secondToLastDay = End.AddDays(-1);
            if (secondDay.Date < End.Date)
                middle = MiddleDays(secondDay, secondToLastDay);

            // Last date is calculated if it is not the same as the start date
            // (because if it is, it would already be calculated with the first day)
            TimeSpan last = new TimeSpan();
            if (End.Date > Start.Date)
                last = LastDay(End);

            return first + middle + last;
        }

        private TimeSpan FirstDay(DateTime Start, DateTime End)
        {
            // obvious cases where the timespan does not touch lunch on the first day
            bool end_sameDay_before_lunch = End.Date == Start.Date && End.TimeOfDay < LunchStart.TimeOfDay;
            if (!IsWorkingDay(Start) | Start.TimeOfDay > LunchEnd.TimeOfDay | end_sameDay_before_lunch)
                return new TimeSpan();

            // cases where the timespan covers the entirety of lunch
            bool start_before_lunch = Start.TimeOfDay < LunchStart.TimeOfDay;
            bool end_after_lunch = (End.Date > Start.Date) | (End.Date == Start.Date && End.TimeOfDay > LunchEnd.TimeOfDay);
            if (start_before_lunch && end_after_lunch)
                return LunchLength();

            // Start before lunch, but end in the middle of lunch
            if (start_before_lunch)
                return End.TimeOfDay - LunchStart.TimeOfDay;

            // Start in the middle of lunch, end after lunch
            if (end_after_lunch)
                return LunchEnd.TimeOfDay - Start.TimeOfDay;

            // Start in the middle of lunch, end in the middle of lunch
            return End.AddMinutes(1).TimeOfDay - Start.TimeOfDay;
        }

        private TimeSpan LastDay(DateTime End)
        {
            // obvious cases where the timespan does not touch lunch on the final day
            if (!IsWorkingDay(End) | End.TimeOfDay < LunchStart.TimeOfDay)
                return new TimeSpan();

            // End time is after lunch on the final day
            if (End.TimeOfDay > LunchEnd.TimeOfDay)
                return LunchLength();

            // End time is partway through lunch
            return End.TimeOfDay - LunchStart.TimeOfDay;
        }

        private TimeSpan MiddleDays(DateTime secondDay, DateTime secondToLastDay)
        {
            DateRange rng = new DateRange(secondDay, secondToLastDay);
            int workingdays = rng.Where(x => IsWorkingDay(x)).Count();
            return TimeSpan.FromTicks(LunchLength().Ticks * workingdays);
        }

        private bool IsWorkingDay(DateTime day)
        {
            if (!weekdays[day.DayOfWeek])
                return false;

            return !Holidays.Where(x => x.Date == day.Date).Any();
        }

        private TimeSpan LunchLength()
        {
            return LunchEnd - LunchStart;
        }

        /// <summary>
        /// Set whether a specific day of the week should be considered a weekday or not.
        /// Weekdays count toward lunchtimes, whereas weekends do not.
        /// </summary>
        /// <param name="Day">Day of the week to set.</param>
        /// <param name="IsWorkDay">True if the day is a weekday (default: Monday-Friday),
        /// false if it is a weekend (default: Saturday/Sunday).</param>
        public void SetWorkday(DayOfWeek Day, bool IsWorkDay)
        {
            weekdays[Day] = IsWorkDay;
        }

        /// <summary>
        /// Identical to SetWorkday, except it allows you to set multiple days at once
        /// by passing in a dictionary of DayOfWeek with their true/false values.
        /// </summary>
        /// <param name="Days">Days of the week to modify from their default values.</param>
        public void SetWorkdays(Dictionary<DayOfWeek, bool> Days)
        {
            foreach (DayOfWeek d in Days.Keys)
                weekdays[d] = Days[d];
        }
    }
}
