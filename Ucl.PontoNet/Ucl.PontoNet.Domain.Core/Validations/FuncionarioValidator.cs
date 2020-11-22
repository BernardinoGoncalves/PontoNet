using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Ucl.PontoNet.Domain.Entities;

namespace Ucl.PontoNet.Domain.Core.Validations
{
    public class FuncionarioValidator : AbstractValidator<Funcionario>
    {
        public FuncionarioValidator()
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
