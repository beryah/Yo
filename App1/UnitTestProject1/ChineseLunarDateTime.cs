using System;

namespace UnitTestProject1
{
    public class ChineseLunarDateTime
    {
        public ChineseLunarDateTime(int year, int month, int day, bool isLeap)
        {
            this.IsLeap = isLeap;
            this.Date = new DateTime(year, month, day);
        }

        /// <summary>
        /// 是否為閏月
        /// </summary>
        public bool IsLeap { get; private set; }

        public DateTime Date { get; private set; }
    }
}