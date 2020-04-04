using System;
using System.Timers;
using System.Globalization;
using KTTTDataInterface;

namespace KTTTApp
{
    public class WorkDay
    {
        private WorkDayModel entryToday { get; set; }
        
        private const int TIMER_INTERVAL = 600000; // in ms

        private Timer timer = null; 

        private readonly CultureInfo culture;

        private readonly IDataAccess dbAccess;

        public WorkDay(in CultureInfo cultureInf, in IDataAccess dataAcc)
        {
            culture = cultureInf;
            dbAccess = dataAcc;

            timer = new System.Timers.Timer();
            timer.Interval = TIMER_INTERVAL;
            timer.Elapsed += ProgressTime;
            timer.AutoReset = true;
            timer.Enabled = true; // start timer

            // generate starting entry
            entryToday = generateNewEntry(culture: culture);

            dbAccess.StoreEntry(entryToday);
        }

        private void ProgressTime(Object source, System.Timers.ElapsedEventArgs e)
        {
            var now = generateNewEntry(culture: culture);
            // for those that work past midnight
            if (now.date != entryToday.date)
            {
                entryToday = now;
                dbAccess.StoreEntry(entryToday);
            } else {
                dbAccess.UpdateEntry(now);
            }
        }

        private int getCalenderWeek(in DateTime date, in CultureInfo culture)
        {
            int retVal = -1;
            try
            {
                Calendar myCal = culture.Calendar;
                retVal = myCal.GetWeekOfYear(date, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek);
            } catch (System.ArgumentOutOfRangeException)
            {
                Console.WriteLine($"Error 2: Failed to get calender week number.");
            }
            return retVal;
        }

        private WorkDayModel generateNewEntry(in CultureInfo culture)
        {
            DateTime startWork = DateTime.Now;

            return new WorkDayModel{
                calWeek = getCalenderWeek(startWork, culture),
                date = startWork.ToString("ddd dd.MM.yyyy", culture),
                startTime = startWork.ToString("T", culture),
                endTime = startWork.ToString("T", culture),
                hoursActive = 0
            };
        }

    }
}
