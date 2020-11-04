using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Ucl.PontoNet.Api.Filters
{
    public class InterceptorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public InterceptorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                var userLangs = context.Request.Headers["Accept-Language"].ToString();
                var lags = userLangs.Split(',');

                if (lags.Length > 0)
                {
                    string language = lags.First();
                    var culture = CultureInfo.CreateSpecificCulture(language);

                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                }

                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            var mensages = new List<string>();

            //if (exception is UnauthorizedAccessException) code = HttpStatusCode.NotFound;
            if (exception is NotImplementedException) code = HttpStatusCode.NotImplemented;
            else if (exception is UnauthorizedAccessException) code = HttpStatusCode.Unauthorized;
            else if (exception is ValidationException)
            {
                code = HttpStatusCode.BadRequest;
                foreach (var item in ((ValidationException)exception).Errors)
                {
                    mensages.Add(item.ErrorMessage);
                }
            };

            if (!mensages.Any())
                mensages.Add(exception.Message);

            var result = JsonConvert.SerializeObject(new Error() { status = (int)code, messages = mensages });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}