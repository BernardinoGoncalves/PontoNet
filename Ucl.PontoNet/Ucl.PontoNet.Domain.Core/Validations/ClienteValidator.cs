using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Ucl.PontoNet.Domain.Entities;

namespace Ucl.PontoNet.Domain.Core.Validations
{
    public class ClienteValidator : AbstractValidator<Cliente>
    {
        public ClienteValidator()
        {
            RuleSet("Insert", () =>
            {

            });

            RuleSet("Update", () =>
            { 

            });
        }
    }
}
