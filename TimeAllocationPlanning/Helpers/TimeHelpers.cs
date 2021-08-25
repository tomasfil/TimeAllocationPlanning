using System;
using System.Collections.Generic;
using System.Text;

namespace TimeAllocationPlanning.Helpers
{
    public static class TimeHelpers
    {
        public static (int hours, int minutes) NormalizeTimeValues(int hours, int minutes)
        {
            while (minutes >= 60)
            {
                minutes -= 60;
                hours++;
            }

            return (hours, minutes);
        }
    }
}
