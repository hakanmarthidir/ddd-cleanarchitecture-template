using DDDTemplate.Application.Abstraction.Response.Enums;

namespace DDDTemplate.Application.Abstraction.Response
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
