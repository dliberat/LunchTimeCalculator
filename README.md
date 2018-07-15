# Lunch Time Calculator

## Background

This project began because I needed a way to assign deadlines to particular tasks based on estimates of how long it would take workers to complete them.
There are a [few](https://www.codeproject.com/Tips/173154/Calculate-Business-Hours) [solutions](https://www.codeproject.com/Tips/991678/Calculate-Business-Hours-Between-Two-Dates-in-Csha)
out there to tackle the problem of working around business hours, which meant that I could now simply calculate:

```
   work start time + estimated time to completion = deadline
```

without having to worry about weekends, closing hours, or holidays. That was a great start.

## But what about lunch?

The problem with that calculation is that people don't generally work 8 hours a day non-stop. My workers all took a 1 hour lunch break every day. That meant that on large
tasks that spanned several days, the calculated deadline could come in several hours shorter than what it should be. And shorter tasks were even worse! I might assign a task
right as lunch time started, and then the deadline would be up before the worker was even back from their lunch break!

I realized that I needed another step in the calculation:

```
   work start time + estimated time to completion = deadline
   deadline + hours devoted to lunch between work start and deadline  = real deadline
```

## Getting Started

Instantiate a `LunchTimeCalculator`, and provide it a start and end date.
By default, `LunchTimeCalculator` will assume that lunch time is between 13:00 and 14:00, and that workers don't work on Saturdays and Sundays.

```
   LunchTimeCalculator ltc = new LunchTimeCalculator();

   DateTime start = DateTime.Parse("2018-08-10 09:00");
   DateTime end = DateTime.Parse("2018-08-10 14:30");
   TimeSpan span = ltc.LunchHoursInSpan(start, end);

   // span.TotalMinutes = 60
```

If the range of dates crosses a weekend or a holiday, those days won't count toward the total lunch time.

```
   LunchTimeCalculator ltc = new LunchTimeCalculator();

   DateTime start = DateTime.Parse("2018-08-13 09:00");
   DateTime end = DateTime.Parse("2018-08-16 18:30");
   TimeSpan span = ltc.LunchHoursInSpan(start, end);

   // 1hr on the 13th + 1hr on the 16th. 14th/15th are Sat/Sun
   // span.TotalMinutes = 120
```

## Modifying Lunch Hours

The `LunchTimeCalculator` provides access to its `LunchStart` and `LunchEnd` attributes, which can be modified as needed. Note that although 
these are DateTime objects, only the `TimeOfDay` portion of the object will be evaluated.

```
	LunchTimeCalculator ltc = new LunchTimeCalculator();
	ltc.LunchStart = DateTime.Parse("12:30");
	ltc.LunchEnd = DateTime.Parse("13:00");

    DateTime start = DateTime.Parse("2018-08-10 09:00");
    DateTime end = DateTime.Parse("2018-08-10 14:30");
    TimeSpan span = ltc.LunchHoursInSpan(start, end);

    // span.TotalMinutes = 30
```


## Adding Holidays

National holidays and days off work can vary by region, so by default this module does not provide any holiday definitions. Provide your own by simply adding in
the relevant dates to the Holidays list.

```
   LunchTimeCalculator ltc = new LunchTimeCalculator();
   ltc.Holidays.Add(DateTime.Parse("2018-12-24"));
   ltc.Holidays.Add(DateTime.Parse("2018-12-25"));

   DateTime start = DateTime.Parse("2018-12-21 13:45");
   DateTime end = DateTime.Parse("2018-12-27 13:15");
   TimeSpan span = ltc.LunchHoursInSpan(start, end);
   
   // 15 mins on the 21st + 60 mins on the 26th + 15 mins on the 27th.
   // 22nd and 23rd are Sat/Sun, 24th and 25th are holidays.
   // span.TotalMinutes = 90
```

## Irregular Workweek

The default workweek is from Monday to Friday, with Saturday and Sunday off. It is possible to modify the internal workweek if something different is required.

```
   // 6 day workweek!
   LunchTimeCalculator ltc = new LunchTimeCalculator();
   // Pass true if it is a workday, false if it is part of the weekend
   ltc.SetWorkday(DayOfWeek.Saturday, true);

   DateTime start = DateTime.Parse("2018-07-20 09:30");
   DateTime end = DateTime.Parse("2018-07-24 14:00");
   TimeSpan span = ltc.LunchHoursInSpan(start, end);
   Assert.AreEqual(240, span.TotalMinutes);
```

The same result can be achieved by passing a dictionary to the `SetWorkdays()` method.

```
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

   // span.TotalMinutes = 0
```

## Contributing

Any and all contributions are welcome. Post an issue or send in a PR!

## License

MIT