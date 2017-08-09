using System;
using System.Collections.Generic;

namespace MarcusUndAnneMod.Models
{
    public class Notification
    {
        public string Text { get; set; }

        public List<int> Times { get; set; }

        public List<DayOfWeek> DaysOfWeek { get; set; }

        public List<int> Days { get; set; }

        public int Chance { get; set; }

        public int Category { get; set; }

        public ModActionTypes Action { get; set; }

        public string ActionParameter { get; set; }

        public string Season { get; set; }
    }
}
