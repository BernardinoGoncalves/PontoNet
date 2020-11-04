using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using FluentValidation.TestHelper;
using Ucl.PontoNet.Domain.Core.Validations;
using Xunit;
using Xunit.Extensions;
using FluentAssertions;
using Ucl.PontoNet.Domain.Entities;
using Ucl.PontoNet.Domain.Enumerable;

namespace Ucl.PontoNet.Test.Validations
{
    public class PersonSampleValidationTest
    {
        private readonly Faker _faker;
        private readonly PersonSample _personSample;
        private readonly PersonSampleValidator _validator;
        public PersonSampleValidationTest()
        {
            _faker = new Faker("en");

            _personSample = new Faker<PersonSample>()
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                .RuleFor(u => u.DateBirth, (f, u) => f.Date.Past(50))
                .RuleFor(u => u.Type, (f, u) => f.PickRandom<PersonTypeSampleEnum>())
                .RuleFor(u => u.Active, (f, u) => f.Random.Bool());

            _validator = new PersonSampleValidator();
        }

        #region Id

        [Fact]
        public void ValidateId_IdEmpty_Message()
        {
            _personSample.Id = Guid.Empty;
            _validator.ShouldHaveValidationErrorFor(person => person.Id, _personSample);
        }




        [Fact]
        public void ValidateId_Id_Success()
        {
            _personSample.Id = Guid.NewGuid();
            _validator.ShouldNotHaveValidationErrorFor(person => person.FirstName, _personSample);
        }
        #endregion

        #region First Name

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ValidateFirstName_FirstNameNullEmpty_Message(string firstName)
        {
            _personSample.FirstName = firstName;
            _validator.ShouldHaveValidationErrorFor(person => person.FirstName, _personSample)
                .WithErrorMessage(Domain.Resources.Validations.PersonSampleFirstNameRequired);
        }

        [Fact]
        public void ValidateFirstName_FirstNameLengthGreaterThan50_Message()
        {
            _personSample.FirstName = this._faker.Random.String(51);
            _validator.ShouldHaveValidationErrorFor(person => person.FirstName, _personSample)
                .WithErrorMessage(Domain.Resources.Validations.PersonSampleFirstNameLength);
        }


        [Theory]
        [InlineData("Leonardo")]
        [InlineData("Leonardo Victor")]
        public void ValidateFirstName_FirstName_Success(string firstName)
        {
            _personSample.FirstName = firstName;
            _validator.ShouldNotHaveValidationErrorFor(person => person.FirstName, _personSample);
        }

        #endregion

        #region Last Name

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ValidateLastName_LastNameNullEmpty_Message(string lastName)
        {
            _personSample.LastName = lastName;
            _validator.ShouldHaveValidationErrorFor(person => person.LastName, _personSample);
        }

        [Fact]
        public void ValidateLastName_LastNameLengthGreaterThan50_Message()
        {
            _personSample.LastName = this._faker.Random.String(51);
            _validator.ShouldHaveValidationErrorFor(person => person.LastName, _personSample);
        }


        [Theory]
        [InlineData("Victor")]
        [InlineData("Victor Balarini")]
        public void ValidateLastName_LastName_Success(string lastName)
        {
            _personSample.LastName = lastName;
            _validator.ShouldNotHaveValidationErrorFor(person => person.LastName, _personSample);
        }

        #endregion

        #region Date Birth


        public static readonly List<object[]> IncorrectData = new List<object[]>
        {
            new object[]{ DateTime.MinValue },
            new object[]{ null }
        };

        public static readonly List<object[]> ValidData = new List<object[]>
        {
            new object[]{ DateTime.MaxValue },
            new object[]{ DateTime.Now },
        };

        [Theory]
        [MemberData(nameof(IncorrectData))]
        public void ValidateDateBirth_DateBirthNullEmpty_Message(DateTime dateBirth)
        {
            _personSample.DateBirth = dateBirth;
            _validator.ShouldHaveValidationErrorFor(person => person.DateBirth, _personSample);
        }


        [Theory]
        [MemberData(nameof(ValidData))]
        public void ValidateDateBirth_DateBirth_Success(DateTime dateBirth)
        {
            _personSample.DateBirth = dateBirth;
            _validator.ShouldNotHaveValidationErrorFor(person => person.DateBirth, _personSample);
        }
        #endregion

        #region Type

        [Fact]
        public void ValidateType_Type_Success()
        {
            _personSample.Type = this._faker.PickRandom<PersonTypeSampleEnum>();
            _validator.ShouldNotHaveValidationErrorFor(person => person.Type, _personSample);
        }



        #endregion

        #region Age

        [Fact]
        public void ValidateAge_Age_Success()
        {
            _validator.ShouldNotHaveValidationErrorFor(person => person.Type, _personSample);
        }

        #endregion
    }
}
