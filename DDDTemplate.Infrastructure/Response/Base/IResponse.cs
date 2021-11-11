using DDDTemplate.Infrastructure.Response.Enums;

namespace DDDTemplate.Infrastructure.Response.Base
{
    public interface IResponse
    {
        ResponseStatus Status { get; set; }
        string Message { get; set; }
    }

    public interface IServiceResponse : IResponse { }
    public interface IServiceResponse<T> : IResponse
    {
        T Data { get; set; }
    }

    public interface ISuccessfulResponse : IServiceResponse { }
    public interface ISuccessfulResponse<T> : IServiceResponse<T> { }

    public interface IFailedServiceResponse : IServiceResponse
    {
        ErrorCodes ServiceErrorCode { get; set; }
    }
    public interface IFailedResponse : IServiceResponse, IFailedServiceResponse { }
    public interface IFailedResponse<T> : IServiceResponse<T>, IFailedServiceResponse { }

}
