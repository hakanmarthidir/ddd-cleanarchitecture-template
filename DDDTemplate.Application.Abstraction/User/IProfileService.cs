using System;
using System.Threading.Tasks;
using DDDTemplate.Application.Contracts.Profile.Request;
using DDDTemplate.Application.Contracts.Profile.Response;
using DDDTemplate.Infrastructure.Response.Base;

namespace DDDTemplate.Application.Abstraction.User
{
    public interface IProfileService
    {
        Task<IServiceResponse<UserProfileDto>> GetProfilebyIdAsync<T>(Guid? userId);
        Task<IServiceResponse<UserProfileDto>> GetProfilebyEmailAsync<T>(string userEmail);
        Task<IServiceResponse> UpdateProfileAsync(Guid? userId, UserProfileUpdateDto profile);
        Task<IServiceResponse> UpdateActivationCodeAsync(Guid? userId, string userActivationCode);
        Task<IServiceResponse> DeleteProfileAsync(Guid? userId);
        Task<IServiceResponse<bool>> GetProfileLicenseStatusbyIdAsync(Guid? userId);
    }
}
