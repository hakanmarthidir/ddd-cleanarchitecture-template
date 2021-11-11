using System;
using System.Threading.Tasks;
using DDDTemplate.Application.Contracts.Profile.Request;
using DDDTemplate.Application.Contracts.Profile.Response;
using DDDTemplate.Infrastructure.Response.Base;

namespace DDDTemplate.Application.Abstraction.Profile
{
    public interface IProfileService
    {
        Task<IServiceResponse<UserProfileResponse>> GetProfilebyIdAsync<T>(Guid? userId);
        Task<IServiceResponse<UserProfileResponse>> GetProfilebyEmailAsync<T>(string userEmail);
        Task<IServiceResponse> UpdateProfileAsync(Guid? userId, UserProfileUpdateRequest profile);
        Task<IServiceResponse> UpdateActivationCodeAsync(Guid? userId, string userActivationCode);
        Task<IServiceResponse> DeleteProfileAsync(Guid? userId);
        Task<IServiceResponse<bool>> GetProfileLicenseStatusbyIdAsync(Guid? userId);
    }
}
