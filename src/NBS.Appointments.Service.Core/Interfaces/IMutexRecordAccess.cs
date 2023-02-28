namespace NBS.Appointments.Service.Core
{
    public interface IMutexRecordAccess : IDisposable
    {
        string Read();
        void Write(string content);
    }
}