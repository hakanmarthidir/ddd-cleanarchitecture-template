using System;
using System.Threading.Tasks;
using DDDTemplate.Application.Abstraction.Profile;
using DDDTemplate.Application.Contracts.Profile.Request;
using DDDTemplate.Application.Contracts.Profile.Response;
using DDDTemplate.Infrastructure.Response.Base;

namespace DDDTemplate.Application.User
{
    public class ProfileService : IProfileService
    {
        public Task<IServiceResponse> DeleteProfileAsync(Guid? userId)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResponse<UserProfileResponse>> GetProfilebyEmailAsync<T>(string userEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResponse<UserProfileResponse>> GetProfilebyIdAsync<T>(Guid? userId)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResponse<bool>> GetProfileLicenseStatusbyIdAsync(Guid? userId)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResponse> UpdateActivationCodeAsync(Guid? userId, string userActivationCode)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResponse> UpdateProfileAsync(Guid? userId, UserProfileUpdateRequest profile)
        {
            throw new NotImplementedException();
        }
    }
}
