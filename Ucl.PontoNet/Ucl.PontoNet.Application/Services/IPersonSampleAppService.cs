using System;
using System.Collections.Generic;
using Ucl.PontoNet.Application.Base;
using Ucl.PontoNet.Application.Dto;
using Ucl.PontoNet.Domain.Base.Interfaces;
using Ucl.PontoNet.Domain.Services;

namespace Ucl.PontoNet.Application.Services
{
    public interface IPersonSampleAppService
    {
        IPersonSampleService personSampleService { get; set; }

        void Delete(Guid id);
        PersonSampleDto GetById(Guid id);
        PersonSampleDto Insert(PersonSampleDto obj);
        PersonSampleDto Update(Guid id, PersonSampleDto obj);
        IEnumerable<PersonSampleDto> Get(bool? active, FilterDto request, out int total);
        IEnumerable<PersonSampleDto> GetAll(FilterDto parameters, out int total);
    }
}