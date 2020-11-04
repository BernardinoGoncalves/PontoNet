using AutoMapper;
using System.Linq;
using Ucl.PontoNet.Application.Dto;
using Ucl.PontoNet.Domain.Entities;

namespace Ucl.PontoNet.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<PersonSample, PersonSampleDto>();
        }
    }
}