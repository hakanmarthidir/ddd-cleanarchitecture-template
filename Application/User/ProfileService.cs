using System;
using System.Threading.Tasks;
using Application.Abstraction.Attributes;
using Application.Abstraction.Response;
using Application.Abstraction.User;
using Application.Contracts.Profile.Request;
using Application.Contracts.Profile.Response;

namespace Application.User
{
    [Log(true)]
    [Performance]
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
