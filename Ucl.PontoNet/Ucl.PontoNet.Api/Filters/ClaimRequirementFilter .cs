
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ucl.PontoNet.Domain.Enumerable;

namespace Ucl.PontoNet.Api.Filters
{
    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly RoleEnum[] roles;

        public ClaimRequirementFilter(RoleEnum[] roles)
        {
            this.roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var claims = context.HttpContext.User.Claims.Where(c => (c.Type == "groupMembership" && this.roles.Any(r => c.Value.ToLower().StartsWith("cn=" + r.ToString().ToLower())))
                                                                 || (c.Type == "groupMembership" && c.Value.ToLower().StartsWith("cn=" + RoleEnum.Administrator.ToString().ToLower()))
                                                                  );
            if (!claims.Any())
            {
                context.Result = new ForbidResult();
            }
            else
            {
                if (!claims.Any(c => c.Value.ToLower().StartsWith("cn=" + RoleEnum.Administrator.ToString().ToLower()))
                 && !claims.Any(c => c.Value.ToLower().StartsWith("cn=" + RoleEnum.User.ToString().ToLower())))
                {

                    foreach (var item in claims)
                    {
                        var role = item.Value.Split(',')[0];
                        if (role.ToLower() != "cn=" + RoleEnum.Administrator.ToString().ToLower() &&
                            role.ToLower() != "cn=" + RoleEnum.User.ToString().ToLower())
                        {
                            context.HttpContext.Items.Add("center", role.Split('_').Last());
                        }

                    }
                }

            }
        }
    }
}