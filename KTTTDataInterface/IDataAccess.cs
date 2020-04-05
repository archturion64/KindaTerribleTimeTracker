using System.Collections.Generic;

namespace KTTTDataInterface
{
    /// <summary>
    /// Definition of basic functionality, a Data storage provider Object should expose.
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        /// Dump all stored data.
        /// </summary>
        /// <returns>all stored entries in a list</returns>
        List<WorkDayModel> GetEntries();

        /// <summary>
        /// Add a new entry.
        /// </summary>
        /// <param name="entry">Data entry that should be stored.</param>
        void StoreEntry(in WorkDayModel entry);

        /// <summary>
        /// Update an entry.
        /// </summary>
        /// <param name="entry">Alleady existing entry that should be updated.</param>
        void UpdateEntry(in WorkDayModel entry);
    }
}
