using System;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using FluentAssertions.Common;
using NSubstitute;
using Ucl.PontoNet.Domain.Core;
using Ucl.PontoNet.Domain.Entities;
using Ucl.PontoNet.Domain.Enumerable;
using Ucl.PontoNet.Domain.Repositories.Interfaces;
using Xunit;
using Ucl.PontoNet.Domain.Base.Interfaces;

namespace Ucl.PontoNet.Test.Services
{
    public class PersonSampleServiceTest
    {

        private Faker _faker;
        private PersonSample _personSample;
        private PersonSampleService _personSampleService;
        private IFixture _fixture;
        private IPersonSampleRepository iPersonSampleRepository;
        private IUnitOfWork iUnitOfWork;

        public PersonSampleServiceTest()
        {
            _faker = new Faker();
            _personSample = new Faker<PersonSample>()
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                .RuleFor(u => u.DateBirth, (f, u) => f.Date.Past(50))
                .RuleFor(u => u.Type, (f, u) => f.PickRandom<PersonTypeSampleEnum>())
                .RuleFor(u => u.Active, (f, u) => f.Random.Bool());

            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });

            iPersonSampleRepository = _fixture.Freeze<IPersonSampleRepository>();
            iUnitOfWork = _fixture.Freeze<IUnitOfWork>();
        }

        #region Insert

        [Theory]
        [InlineData(-5)]
        [InlineData(-17)]
        [InlineData(0)]
        public void ValidateInsert_Under18_Message(int years)
        {

            _personSample.DateBirth = DateTime.Now.AddYears(years);
            iPersonSampleRepository.Insert(_personSample).Returns(_personSample);
            _personSampleService = _fixture.Create<PersonSampleService>();

            _personSampleService.Invoking(y => y.Insert(_personSample))
                .Should().Throw<FluentValidation.ValidationException>()
                .WithMessage("Registration is not allowed to the under 18 years");

        }

        [Fact]
        public void ValidateInsert_Validating_Message()
        {
            _personSample = new PersonSample();
            iPersonSampleRepository.Insert(_personSample).Returns(_personSample);
            _personSampleService = _fixture.Create<PersonSampleService>();

            iUnitOfWork.ValidateEntity = true;
            _personSampleService.Invoking(y => y.Insert(_personSample))
                .Should().Throw<FluentValidation.ValidationException>();

        }

        [Theory]
        [InlineData(-18)]
        [InlineData(-27)]
        [InlineData(-99)]
        public void ValidateInsert_Success(int years)
        {
            _personSample.DateBirth = DateTime.Now.AddYears(years);
            iPersonSampleRepository.Insert(_personSample).Returns(x =>
            {
                _personSample.Id = _faker.Random.Guid();
                return _personSample;
            });
            _personSampleService = _fixture.Create<PersonSampleService>();

            var person = _personSampleService.Invoking(y => y.Insert(_personSample))
                .Should().NotThrow().Subject;

            person.Should().Match<PersonSample>((x) =>
                    x.FirstName == _personSample.FirstName &&
                    x.LastName == _personSample.LastName &&
                    x.DateBirth == _personSample.DateBirth &&
                    x.Active == true &&
                    x.Id != Guid.Empty
                );
        }

        #endregion

    }
}
