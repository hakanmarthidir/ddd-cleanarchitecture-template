using DDDTemplate.Infrastructure.Response.Base;
using DDDTemplate.Infrastructure.Response.Enums;

namespace DDDTemplate.Infrastructure.Response
{
    public class ResponseService : IResponseService
    {
        private string defaultErrorMessage = "An unknown error has occurred while executing the request.";
        private string defaultSuccessMessage = "Successfully Completed.";


        public ISuccessfulResponse SuccessfulResponse(string message = null)
        {
            return new SuccessfulServiceResponse()
            {
                Message = message ?? defaultSuccessMessage,
                Status = ResponseStatus.Success
            };
        }

        public ISuccessfulResponse<T> SuccessfulDataResponse<T>(T data, string message = null)
        {
            return new DataSuccessfulServiceResponse<T>()
            {
                Message = message ?? defaultSuccessMessage,
                Status = ResponseStatus.Success,
                Data = data
            };
        }

        public IFailedResponse FailedResponse(ErrorCodes errorCode, string message = null)
        {
            return new FailedServiceResponse()
            {
                Message = message ?? defaultErrorMessage,
                ServiceErrorCode = errorCode,
                Status = ResponseStatus.Failed
            };
        }

        public IFailedResponse<T> FailedDataResponse<T>(T data, ErrorCodes errorCode, string message = null)
        {
            return new DataFailedServiceResponse<T>()
            {
                Message = message ?? defaultErrorMessage,
                Status = ResponseStatus.Failed,
                Data = data,
                ServiceErrorCode = errorCode
            };
        }


    }
}

