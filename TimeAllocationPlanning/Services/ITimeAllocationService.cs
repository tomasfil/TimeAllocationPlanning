using System;
using System.Collections.Generic;
using TimeAllocationPlanning.Models;

namespace TimeAllocationPlanning.Services
{
    public interface ITimeAllocationService
    {
        int DayHourEnd { get; }
        int DayHourStart { get; }

        TimeFrame Allocate(int requiredTotalMinutes, DateTime date, IEnumerable<TimeFrame> existingAllocations);
        List<TimeFrame> GetEmptyTimeFrames(List<TimeFrame> dayAllocations, DateTime date);
    }
}