using AutoMapper;
using Request.API.Applications.Queries;
using Request.Domain.Entities.Requests;

namespace Request.API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Stage, StageResponse>();
        }
    }
}