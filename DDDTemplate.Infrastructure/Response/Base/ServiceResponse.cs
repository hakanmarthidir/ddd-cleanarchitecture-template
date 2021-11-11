using System;
using DDDTemplate.Infrastructure.Response.Enums;

namespace DDDTemplate.Infrastructure.Response.Base
{

    public class SuccessfulServiceResponse : ISuccessfulResponse
    {
        public ResponseStatus Status { get; set; }
        public string Message { get; set; }
    }

    public class DataSuccessfulServiceResponse<T> : ISuccessfulResponse<T>
    {
        public T Data { get; set; }
        public ResponseStatus Status { get; set; }
        public string Message { get; set; }
    }

    public class FailedServiceResponse : IFailedResponse
    {
        public ErrorCodes ServiceErrorCode { get; set; }
        public ResponseStatus Status { get; set; }
        public string Message { get; set; }
    }

    public class DataFailedServiceResponse<T> : IFailedResponse<T>
    {
        public T Data { get; set; }
        public ErrorCodes ServiceErrorCode { get; set; }
        public ResponseStatus Status { get; set; }
        public string Message { get; set; }
    }
}
