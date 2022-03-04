using Application.Contracts.Auth.Response;
using AutoMapper;

namespace Application.Mappers
{
    public class AutoMappings : Profile
    {
        public AutoMappings()
        {
            // FROM Domain -> TO Dto
            CreateMap<Domain.Entities.UserAggregate.UserActivation, UserLoggedinActivationDto>();
            CreateMap<Domain.Entities.UserAggregate.User, JwtMiddlewareDto>();
            CreateMap<Domain.Entities.UserAggregate.User, UserLoggedinDto>();

            
        }
    }
}
