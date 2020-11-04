using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Ucl.PontoNet.Domain.Enumerable;

namespace Ucl.PontoNet.Api.Filters
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(params RoleEnum[] roles) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] {
                    roles
            };
        }
    }
}
