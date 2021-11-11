using DDDTemplate.Infrastructure.Response.Base;
using DDDTemplate.Infrastructure.Response.Enums;

namespace DDDTemplate.Infrastructure.Response
{
    public interface IResponseService
    {

        ISuccessfulResponse SuccessfulResponse(string message = null);

        ISuccessfulResponse<T> SuccessfulDataResponse<T>(T data, string message = null);

        IFailedResponse FailedResponse(ErrorCodes errorCode, string message = null);

        IFailedResponse<T> FailedDataResponse<T>(T data, ErrorCodes errorCode, string message = null);

    }
}

