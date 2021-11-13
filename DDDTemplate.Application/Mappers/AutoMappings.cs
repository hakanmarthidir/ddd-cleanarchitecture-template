using System;
using AutoMapper;
using DDDTemplate.Application.Contracts.Auth.Response;

namespace DDDTemplate.Application.Mappers
{
    public class AutoMappings : Profile
    {
        public AutoMappings()
        {

            // FROM Domain -> TO Dto
            CreateMap<Domain.AggregatesModel.UserAggregate.User, JwtMiddlewareDto>();
            CreateMap<Domain.AggregatesModel.UserAggregate.User, UserLoggedinDto>();


        }
    }
}
