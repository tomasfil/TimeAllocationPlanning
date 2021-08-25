using System;
using System.Collections.Generic;
using System.Linq;
using TimeAllocationPlanning.Models;

namespace TimeAllocationPlanning.Services
{
    public class TimeAllocationService : ITimeAllocationService
    {
        public TimeAllocationService(int dayHourStart, int dayHourEnd)
        {
            DayHourStart = dayHourStart;
            DayHourEnd = dayHourEnd;
        }

        public int DayHourEnd { get; private set; }
        public int DayHourStart { get; private set; }

        public TimeFrame Allocate(int requiredTotalMinutes, DateTime date, IEnumerable<TimeFrame> existingAllocations)
        {
            var dayAllocations = existingAllocations.Where(allocation => allocation.Date == date).OrderBy(o => o.From).ToList();

            int requiredMinutes = requiredTotalMinutes;
            int requiredHours = 0;
            while (requiredMinutes >= 60)
            {
                requiredMinutes -= 60;
                requiredHours++;
            }

            List<TimeFrame> emptyTimeFrames = GetEmptyTimeFrames(dayAllocations, date);

            var availableTimeFrame = emptyTimeFrames.FirstOrDefault(f => f.TotalMinutes >= requiredTotalMinutes);
            if (availableTimeFrame is null)
            {
                return null;
            }

            var futureAllocation = new TimeFrame(date);

            futureAllocation.SetFrom(availableTimeFrame.From.Hour, availableTimeFrame.From.Minute);

            if (availableTimeFrame.From.Minute + requiredMinutes == 60)
            {
                futureAllocation.SetTo(availableTimeFrame.From.Hour + requiredHours + 1, 0);
            }
            else
            {
                futureAllocation.SetTo(availableTimeFrame.From.Hour + requiredHours, availableTimeFrame.From.Minute + requiredMinutes);
            }

            return futureAllocation;
        }

        public List<TimeFrame> GetEmptyTimeFrames(List<TimeFrame> dayAllocations, DateTime date)
        {
            var allocations = dayAllocations.Where(allocation => allocation.Date == date).OrderBy(o => o.From).ToList();
            var emptyAllocations = new List<TimeFrame>();
            for (int i = 0; i < allocations.Count; i++)
            {
                var allocation = allocations[i];
                if (i == 0 && allocation.From.Hour != DayHourStart)
                {
                    var newAllocation = new TimeFrame(allocation.Date);
                    newAllocation.SetFrom(DayHourStart, 0);
                    newAllocation.SetTo(allocation.From.Hour, allocation.From.Minute);
                    emptyAllocations.Add(newAllocation);
                }
                else if (i != allocations.Count - 1 && !(allocation.To.Hour >= allocations[i + 1].From.Hour && allocation.To.Minute >= allocations[i + 1].From.Minute))
                {
                    var nextAllocation = allocations[i + 1];
                    var newAllocation = new TimeFrame(allocation.Date);

                    newAllocation.SetFrom(allocation.To.Hour, allocation.To.Minute);
                    newAllocation.SetTo(nextAllocation.From.Hour, nextAllocation.From.Minute);
                    emptyAllocations.Add(newAllocation);
                }
                else if (i == allocations.Count - 1 && (allocation.To.Hour != DayHourEnd))
                {
                    var newAllocation = new TimeFrame(allocation.Date);

                    newAllocation.SetFrom(allocation.To.Hour, allocation.To.Minute);
                    newAllocation.SetTo(DayHourEnd, 0);
                    emptyAllocations.Add(newAllocation);
                }
            }

            return emptyAllocations;
        }
    }
}