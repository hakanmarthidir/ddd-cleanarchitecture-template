using System;
using System.Threading.Tasks;
using DDDTemplate.Application.Abstraction.Response;
using DDDTemplate.Application.Abstraction.User;
using DDDTemplate.Application.Contracts.Profile.Request;
using DDDTemplate.Application.Contracts.Profile.Response;

namespace DDDTemplate.Application.User
{
    public class ProfileService : IProfileService
    {
        public Task<IServiceResponse> DeleteProfileAsync(Guid? userId)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResponse<UserProfileDto>> GetProfilebyEmailAsync<T>(string userEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResponse<UserProfileDto>> GetProfilebyIdAsync<T>(Guid? userId)
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

        public Task<IServiceResponse> UpdateProfileAsync(Guid? userId, UserProfileUpdateDto profile)
        {
            throw new NotImplementedException();
        }
    }
}
