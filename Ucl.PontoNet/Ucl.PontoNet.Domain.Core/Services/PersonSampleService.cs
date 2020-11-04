using System;
using FluentValidation;
using Ucl.PontoNet.Domain.Base;
using Ucl.PontoNet.Domain.Base.Interfaces;
using Ucl.PontoNet.Domain.Core.Validations;
using Ucl.PontoNet.Domain.Entities;
using Ucl.PontoNet.Domain.Repositories.Interfaces;
using Ucl.PontoNet.Domain.Services;

namespace Ucl.PontoNet.Domain.Core
{
    public class PersonSampleService : Service<PersonSample>, IPersonSampleService
    {
        public PersonSampleService(IUnitOfWork context, IPersonSampleRepository rep) : base(context, rep)
        {
            Validator = new PersonSampleValidator();
        }


        public override PersonSample Insert(PersonSample obj)
        {
            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Insert");

            if ((DateTime.Now.Year - obj.DateBirth.Year) < 18)
                throw new ValidationException("Registration is not allowed to the under 18 years");

            obj.Active = true;

            return Repository.Insert(obj);
        }
    }
}