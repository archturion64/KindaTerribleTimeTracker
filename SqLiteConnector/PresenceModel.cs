using System;

namespace SqLiteConnector
{
    public class PresenceModel
    {
        public int key { get; set; }

        public int calWeek { get; set; }

        public string date { get; set; }

        public string startTime { get; set; }

        public string endTime { get; set; }

        public float hoursActive { get; set; }
    }
}
