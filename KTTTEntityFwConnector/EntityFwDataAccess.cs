using System;
using KTTTDataInterface;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace KTTTEntityFwConnector
{
    /// <summary>
    /// Adaptopr of entity framework iface to IDataAccess from KTTTDataInterface
    /// </summary>
    public class EntityFwDataAccess : IDataAccess
    {
        /// <summary>
        /// db specific sonnection string
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Constructor of Adaptor class
        /// </summary>
        /// <param name="connString"></param>
        public EntityFwDataAccess(in string connString)
        {
            connectionString = connString;
        }

        /// <summary>
        /// Dump table contents.
        /// </summary>
        /// <returns></returns>
        public List<WorkDayModel> GetEntries()
        {
            var retVal = new List<WorkDayModel>();
            using (var dataContext = new WorkdayDBContext(connectionString))
            {
                retVal = dataContext.WorkDay.ToList();
            }

            return retVal;
        }

        /// <summary>
        /// Add new entry or update existing one.
        /// </summary>
        /// <param name="entry"> Data model object</param>
        public void StoreEntry(in WorkDayModel entry)
        {
            using (var dataContext = new WorkdayDBContext(connectionString))
            {
                var matchedEntity = dataContext.WorkDay.Find(entry.date);
                if(matchedEntity == null)
                {
                    dataContext.WorkDay.Add(entry);
                } else {
                    matchedEntity.endTime = entry.endTime;
                    matchedEntity.hoursActive +=  entry.hoursActive;
                }
                
                dataContext.SaveChanges();
            }
        }
    }
}
