using Application.Abstraction.Response.Enums;

namespace Application.Abstraction.Response
{
    public interface IServiceResponse
    {
        ResponseStatus Status { get; set; }
        string Message { get; set; }
        ErrorCodes ErrorCode { get; set; }
    }

    public interface IServiceResponse<T> : IServiceResponse
    {
        T Data { get; set; }
    }

}
