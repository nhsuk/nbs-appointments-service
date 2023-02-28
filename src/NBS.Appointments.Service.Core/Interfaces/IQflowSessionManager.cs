namespace NBS.Appointments.Service.Core
{
    public interface IQflowSessionManager
    {
        string GetSessionId();
        void Invalidate(string sessionId);
    }
}