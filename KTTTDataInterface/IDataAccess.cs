using System.Collections.Generic;

namespace KTTTDataInterface
{
    public interface IDataAccess
    {
        List<WorkDayModel> GetEntries();

        void StoreEntry(in WorkDayModel entry);

        void UpdateEntry(in WorkDayModel entry);
    }
}
