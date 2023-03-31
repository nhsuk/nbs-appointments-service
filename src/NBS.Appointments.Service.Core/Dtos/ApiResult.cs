using System.Net;

namespace NBS.Appointments.Service.Core.Dtos
{
    public class ApiResult<TResponse> where TResponse : class
    {
        public TResponse ResponseData { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
