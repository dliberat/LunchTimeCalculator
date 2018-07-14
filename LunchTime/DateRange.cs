using System;
using System.Collections;
using System.Collections.Generic;

namespace LunchTime
{
    class DateRange : IEnumerable<DateTime>
    {
        protected DateTime _start;
        protected DateTime _end;
        protected DateTime _current;

        public DateRange(DateTime Start, DateTime End)
        {
            _start = Start.Date;
            _end = End.Date;
            _current = _start.AddDays(-1);
        }

        public IEnumerator<DateTime> GetEnumerator()
        {
            while (_current.Date < _end.Date)
            {
                _current = _current.AddDays(1);
                yield return _current;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
