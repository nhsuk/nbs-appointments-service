namespace NBS.Appointments.Service.Core
{
    public interface IMutexRecordStore
    {
        IMutexRecordAccess Acquire(string fileName);
    }
}