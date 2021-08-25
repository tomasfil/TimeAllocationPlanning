using System;
using System.Collections.Generic;
using System.Text;

namespace TimeAllocationPlanning.Models
{
    public class TimeFrame : ITimeFrame
    {
        public TimeFrame(DateTime date)
        {
            Date = date;
        }

        public DateTime Date { get; private set; }
        public DateTime From { get; private set; }

        public DateTime To { get; private set; }

        public int TotalMinutes => (To.Hour - From.Hour) * 60 + (To.Minute - From.Minute);

        public bool CollidesWith(TimeFrame timeAllocation)
        {
            return
                (timeAllocation.From >= From && timeAllocation.From < To) ||
                (timeAllocation.To > From && timeAllocation.To <= To);
        }

        public void SetFrom(int hours, int minutes)
        {
            int hoursToSet, minutesToSet;
            NormalizeTimeValues(hours, minutes, out hoursToSet, out minutesToSet);
            From = new DateTime(Date.Year, Date.Month, Date.Day, hoursToSet, minutesToSet, 0);
        }

        public void SetTo(int hours, int minutes)
        {
            int hoursToSet, minutesToSet;
            NormalizeTimeValues(hours, minutes, out hoursToSet, out minutesToSet);

            To = new DateTime(Date.Year, Date.Month, Date.Day, hoursToSet, minutesToSet, 0);
        }

        private static void NormalizeTimeValues(int hours, int minutes, out int hoursToSet, out int minutesToSet)
        {
            hoursToSet = hours;
            minutesToSet = minutes;
            while (minutesToSet >= 60)
            {
                minutesToSet -= 60;
                hoursToSet++;
            }
        }

        public override string ToString()
        {
            return $"{Date:d}   {From:t} -> {To:t}";
        }
    }
}
