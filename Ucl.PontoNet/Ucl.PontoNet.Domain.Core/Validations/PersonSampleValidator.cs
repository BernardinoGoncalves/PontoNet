using FluentValidation;
using Ucl.PontoNet.Domain.Entities;

namespace Ucl.PontoNet.Domain.Core.Validations
{
    public class PersonSampleValidator : AbstractValidator<PersonSample>
    {
        public PersonSampleValidator()
        {
            ValidateId();
            ValidateFirstName();
            ValidateLastName();
            ValidateDateBirth();
            ValidateType();


            RuleSet("Insert", () =>
            {
                ValidateFirstName();
                ValidateLastName();
                ValidateDateBirth();
                ValidateType();
                ValidateAge();
            });

            RuleSet("Update", () =>
            {
                ValidateId();
                ValidateFirstName();
                ValidateLastName();
                ValidateDateBirth();
                ValidateType();
                ValidateAge();
            });

        }

        #region Validações de campos

        private void ValidateId()
        {
            RuleFor(o => o.Id).NotEmpty().WithMessage(Resources.Validations.PersonSampleIdRequired);
        }

        private void ValidateFirstName()
        {
            RuleFor(o => o.FirstName)
                .NotEmpty().WithMessage(Resources.Validations.PersonSampleFirstNameRequired)
                .Length(1, 50).WithMessage(Resources.Validations.PersonSampleFirstNameLength);
        }

        private void ValidateLastName()
        {
            RuleFor(o => o.LastName)
                .NotEmpty().WithMessage(Resources.Validations.PersonSampleLastNameRequired)
                .Length(1, 50).WithMessage(Resources.Validations.PersonSampleLastNameLength);
        }

        private void ValidateDateBirth()
        {
            RuleFor(o => o.DateBirth)
                .NotEmpty().WithMessage(Resources.Validations.PersonSampleDateBirthRequired);
        }

        private void ValidateType()
        {
            RuleFor(o => o.Type)
                .NotEmpty().WithMessage(Resources.Validations.PersonSampleTypeRequired);
        }

        private void ValidateAge()
        {
            RuleFor(o => o.Age)
                .NotEmpty().WithMessage(Resources.Validations.PersonSampleAgeRequired);
        }


        #endregion Validações de campos
    }
}