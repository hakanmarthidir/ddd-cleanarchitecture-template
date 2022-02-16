using System;
using System.Threading.Tasks;
using Application.Abstraction.Response;
using Application.Contracts.Profile.Request;
using Application.Contracts.Profile.Response;

namespace Application.Abstraction.User
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
