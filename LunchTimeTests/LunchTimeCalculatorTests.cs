using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace LunchTime.Tests
{
    [TestClass()]
    public class LunchTimeCalculatorTests
    {
        [TestMethod()]
        public void LunchHoursInSpan_EndsBeforeLunch()
        {
            LunchTimeCalculator ltc = new LunchTimeCalculator();
            DateTime start = DateTime.Parse("2018-08-10 09:00");
            DateTime end = DateTime.Parse("2018-08-10 11:30");
            TimeSpan span = ltc.LunchHoursInSpan(start, end);
            Assert.AreEqual(0, span.TotalMinutes);
        }

        [TestMethod()]
        public void LunchHoursInSpan_StartsAfterLunch()
        {
            LunchTimeCalculator ltc = new LunchTimeCalculator();
            DateTime start = DateTime.Parse("2018-08-10 14:00");
            DateTime end = DateTime.Parse("2018-08-10 14:30");
            TimeSpan span = ltc.LunchHoursInSpan(start, end);
            Assert.AreEqual(0, span.TotalMinutes);
        }

        [TestMethod()]
        public void LunchHoursInSpan_GoesPartwayIntoLunch()
        {
            LunchTimeCalculator ltc = new LunchTimeCalculator();
            DateTime start = DateTime.Parse("2018-08-10 12:00");
            DateTime end = DateTime.Parse("2018-08-10 13:25");
            TimeSpan span = ltc.LunchHoursInSpan(start, end);
            Assert.AreEqual(25, span.TotalMinutes);
        }

        [TestMethod()]
        public void LunchHoursInSpan_StartsPartwayThruLunch()
        {
            LunchTimeCalculator ltc = new LunchTimeCalculator();
            DateTime start = DateTime.Parse("2018-08-10 13:20");
            DateTime end = DateTime.Parse("2018-08-10 17:00");
            TimeSpan span = ltc.LunchHoursInSpan(start, end);
            Assert.AreEqual(40, span.TotalMinutes);
        }

        [TestMethod()]
        public void LunchHoursInSpan_SpansSingleDayLunch()
        {
            LunchTimeCalculator ltc = new LunchTimeCalculator();
            DateTime start = DateTime.Parse("2018-08-10 12:00");
            DateTime end = DateTime.Parse("2018-08-10 18:30");
            TimeSpan span = ltc.LunchHoursInSpan(start, end);
            Assert.AreEqual(60, span.TotalMinutes);
        }

        [TestMethod()]
        public void LunchHoursInSpan_TwoDays_EndsBeforeLunch()
        {
            LunchTimeCalculator ltc = new LunchTimeCalculator();
            DateTime start = DateTime.Parse("2018-08-08 12:00");
            DateTime end = DateTime.Parse("2018-08-09 10:30");
            TimeSpan span = ltc.LunchHoursInSpan(start, end);
            Assert.AreEqual(60, span.TotalMinutes);
        }

        [TestMethod()]
        public void LunchHoursInSpan_TwoDays_SpansTwoLunches()
        {
            LunchTimeCalculator ltc = new LunchTimeCalculator();
            DateTime start = DateTime.Parse("2018-08-08 12:00");
            DateTime end = DateTime.Parse("2018-08-09 15:00");
            TimeSpan span = ltc.LunchHoursInSpan(start, end);
            Assert.AreEqual(120, span.TotalMinutes);
        }

        [TestMethod()]
        public void LunchHoursInSpan_TwoDays_StartsPartwayThruLunch()
        {
            LunchTimeCalculator ltc = new LunchTimeCalculator();
            DateTime start = DateTime.Parse("2018-08-08 13:50");
            DateTime end = DateTime.Parse("2018-08-09 15:00");
            TimeSpan span = ltc.LunchHoursInSpan(start, end);
            Assert.AreEqual(70, span.TotalMinutes);
        }

        [TestMethod()]
        public void LunchHoursInSpan_TwoDays_EndsPartwayThruLunch()
        {
            LunchTimeCalculator ltc = new LunchTimeCalculator();
            DateTime start = DateTime.Parse("2018-08-08 9:00");
            DateTime end = DateTime.Parse("2018-08-09 13:05");
            TimeSpan span = ltc.LunchHoursInSpan(start, end);
            Assert.AreEqual(65, span.TotalMinutes);
        }

        [TestMethod()]
        public void LunchHoursInSpan_TwoDays_StartsAndEndsPartwayThruLunch()
        {
            LunchTimeCalculator ltc = new LunchTimeCalculator();
            DateTime start = DateTime.Parse("2018-08-07 13:05");
            DateTime end = DateTime.Parse("2018-08-08 13:20");
            TimeSpan span = ltc.LunchHoursInSpan(start, end);
            Assert.AreEqual(75, span.TotalMinutes);
        }

        [TestMethod()]
        public void LunchHoursInSpan_IsWeekend()
        {
            LunchTimeCalculator ltc = new LunchTimeCalculator();
            DateTime start = DateTime.Parse("2018-07-14 09:30");
            DateTime end = DateTime.Parse("2018-07-14 18:30");
            TimeSpan span = ltc.LunchHoursInSpan(start, end);
            Assert.AreEqual(0, span.TotalMinutes);
        }

        [TestMethod()]
        public void LunchHoursInSpan_EndsOnWeekend()
        {
            LunchTimeCalculator ltc = new LunchTimeCalculator();
            DateTime start = DateTime.Parse("2018-07-12 09:30");
            DateTime end = DateTime.Parse("2018-07-15 18:30");
            TimeSpan span = ltc.LunchHoursInSpan(start, end);
            Assert.AreEqual(120, span.TotalMinutes);
        }

        [TestMethod()]
        public void LunchHoursInSpan_StartsOnWeekend()
        {
            LunchTimeCalculator ltc = new LunchTimeCalculator();
            DateTime start = DateTime.Parse("2018-07-22 09:30");
            DateTime end = DateTime.Parse("2018-07-23 18:30");
            TimeSpan span = ltc.LunchHoursInSpan(start, end);
            Assert.AreEqual(60, span.TotalMinutes);
        }

        [TestMethod()]
        public void LunchHoursInSpan_CrossesWeekend()
        {
            LunchTimeCalculator ltc = new LunchTimeCalculator();
            DateTime start = DateTime.Parse("2018-07-20 09:30");
            DateTime end = DateTime.Parse("2018-07-24 13:30");
            TimeSpan span = ltc.LunchHoursInSpan(start, end);
            Assert.AreEqual(150, span.TotalMinutes);
        }

        [TestMethod()]
        public void SetWeekday()
        {
            LunchTimeCalculator ltc = new LunchTimeCalculator();
            ltc.SetWorkday(DayOfWeek.Saturday, true);

            DateTime start = DateTime.Parse("2018-07-20 09:30");
            DateTime end = DateTime.Parse("2018-07-24 14:00");
            TimeSpan span = ltc.LunchHoursInSpan(start, end);
            Assert.AreEqual(240, span.TotalMinutes);
        }

        [TestMethod()]
        public void SetWeekdays()
        {
            Dictionary<DayOfWeek, bool> week = new Dictionary<DayOfWeek, bool>()
            {
                { DayOfWeek.Monday, false },
                { DayOfWeek.Tuesday, false },
                { DayOfWeek.Wednesday, false },
                { DayOfWeek.Thursday, false },
                { DayOfWeek.Friday, false },
                { DayOfWeek.Saturday, false },
                { DayOfWeek.Sunday, false },
            };

            LunchTimeCalculator ltc = new LunchTimeCalculator();
            ltc.SetWorkdays(week);

            DateTime start = DateTime.Parse("2018-07-20 09:30");
            DateTime end = DateTime.Parse("2018-07-30 14:00");
            TimeSpan span = ltc.LunchHoursInSpan(start, end);
            Assert.AreEqual(0, span.TotalMinutes);
        }

        [TestMethod()]
        public void LunchHoursInSpan_SpansHoliday()
        {
            LunchTimeCalculator ltc = new LunchTimeCalculator();
            ltc.Holidays.Add(DateTime.Parse("2018-12-25"));

            DateTime start = DateTime.Parse("2018-12-24 09:30");
            DateTime end = DateTime.Parse("2018-12-26 18:30");
            TimeSpan span = ltc.LunchHoursInSpan(start, end);
            Assert.AreEqual(120, span.TotalMinutes);
        }

        [TestMethod()]
        public void LunchHoursInSpan_WeekendAndHolidayOnly()
        {
            LunchTimeCalculator ltc = new LunchTimeCalculator();
            ltc.Holidays.Add(DateTime.Parse("2018-12-24"));
            ltc.Holidays.Add(DateTime.Parse("2018-12-25"));

            DateTime start = DateTime.Parse("2018-12-22 09:30");
            DateTime end = DateTime.Parse("2018-12-25 18:30");
            TimeSpan span = ltc.LunchHoursInSpan(start, end);
            Assert.AreEqual(0, span.TotalMinutes);
        }

        [TestMethod()]
        public void LunchHoursInSpan_SpansWeekendAndHoliday()
        {
            LunchTimeCalculator ltc = new LunchTimeCalculator();
            ltc.Holidays.Add(DateTime.Parse("2018-12-24"));
            ltc.Holidays.Add(DateTime.Parse("2018-12-25"));

            DateTime start = DateTime.Parse("2018-12-21 13:45");
            DateTime end = DateTime.Parse("2018-12-27 13:15");
            TimeSpan span = ltc.LunchHoursInSpan(start, end);
            Assert.AreEqual(90, span.TotalMinutes);
        }

        [TestMethod()]
        public void LunchHoursInSpan_StartIsEnd()
        {
            LunchTimeCalculator ltc = new LunchTimeCalculator();

            DateTime start = DateTime.Parse("2018-07-12");
            DateTime end = DateTime.Parse("2018-07-12");
            TimeSpan span = ltc.LunchHoursInSpan(start, end);
            Assert.AreEqual(0, span.TotalMinutes);
        }
    }
}