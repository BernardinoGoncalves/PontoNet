using AutoMapper;
using System.Collections.Generic;
using Ucl.PontoNet.Application.Dto;
using Ucl.PontoNet.Domain.Entities;

namespace Ucl.PontoNet.Application.AutoMapper
{
    public class DataTransferToDomainMappingProfile : Profile
    {
        public DataTransferToDomainMappingProfile()
        {

            CreateMap<PersonSampleDto, PersonSample>();
            CreateMap<ClienteDto, Cliente>();
            CreateMap<FuncionarioDto, Funcionario>();

        }
    }
}