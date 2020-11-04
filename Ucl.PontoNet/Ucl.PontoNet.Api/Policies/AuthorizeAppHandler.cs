using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Ucl.PontoNet.Api.Policies
{
    public class AuthorizeAppHandler : AuthorizationHandler<AuthorizeAppRequirement>
    {
        private const string AuthorizeHeader = "Authorization";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizeAppHandler(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(
          AuthorizationHandlerContext context,
          AuthorizeAppRequirement requirement)
        {
            if (!Startup.AuthenticatedEnvironments.Contains(requirement.Environment.ToLower()))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            string token = _httpContextAccessor.HttpContext.Request.Headers[AuthorizeHeader];


            if (token != null && token.Equals(requirement.Token))
                context.Succeed(requirement);
            else
                context.Fail();
            return Task.CompletedTask;
        }
    }
}
