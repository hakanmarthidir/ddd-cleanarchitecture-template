using AutoMapper;
using DDDTemplate.Application.Contracts.Auth.Response;
using DDDTemplate.Domain.Entities.UserAggregate.Enums;

namespace DDDTemplate.Application.Mappers
{
    public class AutoMappings : Profile
    {
        public AutoMappings()
        {
            // FROM Domain -> TO Dto
            CreateMap<Domain.Entities.UserAggregate.User, JwtMiddlewareDto>();

            CreateMap<Domain.Entities.UserAggregate.User, UserLoggedinDto>()
                .ForMember(x => x.UserType, opt => opt.MapFrom(o => o.UserType.Value))
                .ForMember(x => x.IsActivated, opt => opt.MapFrom(o => o.Activation.IsActivated.Value));            
        }
    }  
}
