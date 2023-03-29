namespace NBS.Appointments.Service.Core.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }        
        DateTime LocalNow { get; }
    }
}
