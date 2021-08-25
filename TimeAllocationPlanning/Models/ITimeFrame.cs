using System;

namespace TimeAllocationPlanning.Models
{
    public interface ITimeFrame
    {
        DateTime Date { get; }
        DateTime From { get; }
        DateTime To { get; }
        int TotalMinutes { get; }

        bool CollidesWith(TimeFrame timeAllocation);
        void SetFrom(int hours, int minutes);
        void SetTo(int hours, int minutes);
    }
}