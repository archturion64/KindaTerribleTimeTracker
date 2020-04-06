using System;
using KTTTDataInterface;
using Microsoft.EntityFrameworkCore;


namespace KTTTEntityFwConnector
{
    public class WorkdayDBContext : DbContext
    {   
        /// <summary>
        /// init done flag
        /// </summary>
        private static bool created = false;

        /// <summary>
        /// read only ref needed for db access config
        /// </summary>
        readonly string connectionString;

        public WorkdayDBContext(in string connStr)
        {
            connectionString = connStr;
            if (!created)
            {
                created = true;
                Database.EnsureCreated();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            optionbuilder.UseSqlite(connectionString);
        }

        /// <summary>
        /// Entity
        /// </summary>
        /// <value></value>
        public DbSet<WorkDayModel> WorkDay { get; set; }
    }
}
