using System;
using System.ComponentModel.DataAnnotations;
using Ucl.PontoNet.Domain.Enumerable;

namespace Ucl.PontoNet.Application.Dto
{
    public class PersonSampleDto
    {
        public Guid Id { get; set; }

        /// <summary>
        ///     Person first name
        /// </summary>
        /// <example>Leonardo</example>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        ///     Person last name
        /// </summary>
        /// <example>Balarini</example>
        public string LastName { get; set; }

        /// <summary>
        ///     Person Date Birth
        /// </summary>
        /// <example>1985-01-01</example>
        public DateTime DateBirth { get; set; }

        /// <summary>
        ///     Person type
        /// </summary>
        /// <example>Physical Person = 1</example>
        public PersonTypeSampleEnum Type { get; set; }

        /// <summary>
        ///     is Person active?
        /// </summary>
        /// <example>True</example>
        [Required]
        public bool Active { get; set; }


        /// <summary>
        /// Person age
        /// </summary>
        public int Age { get; set; }
    }

}