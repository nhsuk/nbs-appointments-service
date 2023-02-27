using System.Net;
using NBS.Appointments.Service.Infrastructure.Enum;

namespace NBS.Appointments.Service.Infrastructure.Entities.Api
{
    public class ApiResult
    {
        public ApiResult(HttpStatusCode httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
            Status = MapStatusFrom(httpStatusCode);
        }

        public ApiResponseStatus Status { get; }

        public HttpStatusCode HttpStatusCode { get; }

        private ApiResponseStatus MapStatusFrom(HttpStatusCode httpStatusCode)
        {
            if ((int)httpStatusCode >= 200 && (int)httpStatusCode < 300)
            {
                return ApiResponseStatus.Success;
            }

            if (httpStatusCode == HttpStatusCode.BadRequest)
            {
                return ApiResponseStatus.BadRequest;
            }

            if (httpStatusCode == HttpStatusCode.NotFound)
            {
                return ApiResponseStatus.NotFound;
            }
            return ApiResponseStatus.Other;
        }
    }

    public class ApiResult<TResponseData> : ApiResult
        where TResponseData : class
    {
        public ApiResult(HttpStatusCode httpStatusCode) : this(httpStatusCode, null)
        {
            ResponseData = null;
        }

        public ApiResult(HttpStatusCode httpStatusCode, TResponseData data) : base(httpStatusCode)
        {
            ResponseData = data;
        }

        public TResponseData ResponseData { get; }
    }
}
