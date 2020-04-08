using System;
using System.Timers;
using System.Globalization;
using KTTTDataInterface;

namespace KTTTApp
{
    /// <summary>
    /// Handles the application's business logic.
    /// Progresses the time tracking and stores / updates database entries.
    /// </summary>
    public class WorkDay
    {
        /// <summary>
        /// most current value for WorkDayModel
        /// </summary>
        /// <value>date time</value>
        public DateTime entryToday { get; private set; } = DateTime.Now;

        /// <summary>
        /// mechanism for progressing elapsed time during the work day
        /// </summary>
        private Timer timer = null; 

        /// <summary>
        /// dep injected read-only reference needed for date formating
        /// </summary>
        private readonly CultureInfo culture;

        /// <summary>
        /// dep injected read-only reference needed for DB access
        /// </summary>
        private readonly IDataAccess dbAccess;

        /// <summary>
        /// Constructor that generates and stores a workday DB entry on invokation.
        /// It sets up a timer for DB updates and starts it.
        /// </summary>
        /// <exception cref="System.ArgumentNullException"> if any of the passed params are null</exception>
        /// <param name="cultureInf">a CultureInfo() instance</param>
        /// <param name="dataAcc">object that implements the IDataAccess interface</param>
        public WorkDay(CultureInfo cultureInf, IDataAccess dataAcc)
        {
            if (cultureInf == null)
            {
                throw new System.ArgumentNullException("Parameter cannot be null", "cultureInf");
            } else if (dataAcc == null)
            {
                throw new System.ArgumentNullException("Parameter cannot be null", "dataAcc");
            }

            culture = cultureInf;
            dbAccess = dataAcc;

            timer = new System.Timers.Timer();
            timer.Interval = TimeSpan.FromMinutes((int)ETimerInterval.Minutes).TotalMilliseconds;

            timer.Elapsed += ProgressTime;
            timer.AutoReset = true;
            timer.Enabled = true; // start timer

            // generate starting entry
            dbAccess.StoreEntry(generateNewEntry(culture: culture));
        }

        /// <summary>
        /// Callback function of the timer mechanism.
        /// It gets called every time a TIMER_INTERVAL elapses.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void ProgressTime(Object source, System.Timers.ElapsedEventArgs e)
        {
            dbAccess.StoreEntry(generateNewEntry(culture: culture));
        }

        /// <summary>
        /// Get current calender week number.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="culture"></param>
        /// <returns>Current number of the calender week (1-53) or -1 on error</returns>
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

        /// <summary>
        /// Generate new WorkDayModel with current time and date.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns>new DB model data</returns>
        public WorkDayModel generateNewEntry(in CultureInfo culture)
        {
            DateTime now = DateTime.Now;
            // get time diff from previous stored val;
            TimeSpan interval = now - entryToday;
            
            entryToday = now;

            return new WorkDayModel{
                calWeek = getCalenderWeek(entryToday, culture),
                date = entryToday.ToString("ddd dd.MM.yyyy", culture),
                startTime = entryToday.ToString("T", culture),
                endTime = entryToday.ToString("T", culture),
                hoursActive = (float)Math.Round(interval.TotalHours, 2, MidpointRounding.ToZero)
            };
        }

    }
}
