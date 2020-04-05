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
        /// <value>Model of DB relavant information</value>
        public WorkDayModel entryToday { get; private set; }
        
        /// <summary>
        /// interval in ms used for setting up the timer callback delay
        /// </summary>
        private const int TIMER_INTERVAL = 600000;

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
            timer.Interval = TIMER_INTERVAL;
            timer.Elapsed += ProgressTime;
            timer.AutoReset = true;
            timer.Enabled = true; // start timer

            // generate starting entry
            entryToday = generateNewEntry(culture: culture);

            dbAccess.StoreEntry(entryToday);
        }

        /// <summary>
        /// Callback function of the timer mechanism.
        /// It gets called every time a TIMER_INTERVAL elapses.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
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
