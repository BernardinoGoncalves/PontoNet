using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Ucl.PontoNet.Api.Filters;
using Ucl.PontoNet.Application.Dto;
using Ucl.PontoNet.Application.Services;


namespace Ucl.PontoNet.Api.Policies
{
    public class BasicAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string UserinfoEndpoint { get; set; }
    }
    public class CustomAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private readonly IMemoryCacheWithPolicy<UserInfoDto> _cache;

        public CustomAuthenticationHandler(
            IOptionsMonitor<BasicAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IMemoryCacheWithPolicy<UserInfoDto> cache,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _cache = cache;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {


            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Unauthorized");

            string authorizationHeader = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return AuthenticateResult.NoResult();
            }


            if (!authorizationHeader.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            string token = authorizationHeader.Substring("bearer".Length).Trim();

            if (string.IsNullOrEmpty(token) || token.Length < 10)
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            try
            {
                return await validateTokenAsync(token);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }
        }

        private async Task<AuthenticateResult> validateTokenAsync(string token)
        {
            // var _cache = new MemoryCacheWithPolicy<UserInfoDto>();

            SHA256 shaAlgorithm = new SHA256Managed();
            byte[] shaDigest = shaAlgorithm.ComputeHash(ASCIIEncoding.ASCII.GetBytes(token));
            var hash = BitConverter.ToString(shaDigest);

            var result = await _cache.GetOrCreateAsync(hash, () => requestValidation(token));

            // var result = await requestValidation(token);


            if (result == null)
            {
                return AuthenticateResult.NoResult();
            }

            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, result.cn),
                    // new Claim("groupMembership", JsonConvert.SerializeObject(result.groupMembership)),
                    new Claim(ClaimTypes.NameIdentifier, result.cn ),
                    new Claim(ClaimTypes.GivenName, result.UserFullName ),
                    //new Claim(ClaimTypes.Email, result.mail),
                    //new Claim(ClaimTypes.Country, result.locationCountry),
                };

            var identity = new ClaimsIdentity(claims, ClaimTypes.Name);
            var principal = new System.Security.Principal.GenericPrincipal(identity, null);
            var ticket = new AuthenticationTicket(principal, ClaimTypes.Name);




            return AuthenticateResult.Success(ticket);
        }

        private async Task<UserInfoDto> requestValidation(string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(this.Options.UserinfoEndpoint);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var responseString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<UserInfoDto>(responseString);


            }
        }
    }
}