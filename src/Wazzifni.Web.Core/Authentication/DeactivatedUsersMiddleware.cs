using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wazzifni.Authorization;

namespace Wazzifni.Authentication
{
    public class DeactivatedUsersMiddleware : IMiddleware
    {
        private readonly JwtSecurityTokenHandler handler;
        private readonly DeactivatedUsersSet deactivatedUsersSet;

        private const string UserIdKeyInJwt = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public DeactivatedUsersMiddleware(DeactivatedUsersSet deactivatedUsersSet)
        {
            handler = new JwtSecurityTokenHandler();
            this.deactivatedUsersSet = deactivatedUsersSet;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out var authValue))
            {
                var token = Regex.Replace(authValue.ToString(), "bearer", "", RegexOptions.IgnoreCase).Trim();

                if (!handler.CanReadToken(token))
                {
                    await next(context);
                    return;
                }

                var jwtSecurityToken = handler.ReadJwtToken(token);

                var userIdString = jwtSecurityToken.Claims
                    .FirstOrDefault(claim => claim.Type == UserIdKeyInJwt)
                    ?.Value;

                if (long.TryParse(userIdString, out var userId) && deactivatedUsersSet.IsDeactivated(userId))
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized; // 401 Unauthorized

                    var response = new
                    {
                        result = (object)null,
                        targetUrl = (object)null,
                        success = false,
                        error = new
                        {
                            code = 0,
                            message = "Current user did not log in to the application!",
                            details = "AbpAuthorizationException: Current user did not login to the application!\nSTACK TRACE:    at Abp.Authorization.AuthorizationHelper.AuthorizeAsync(IEnumerable`1 authorizeAttributes)\n   at Abp.Authorization.AuthorizationHelper.CheckPermissionsAsync(MethodInfo methodInfo, Type type)\n   at Abp.Authorization.AuthorizationHelper.AuthorizeAsync(MethodInfo methodInfo, Type type)\n   at Abp.AspNetCore.Mvc.Authorization.AbpAuthorizationFilter.OnAuthorizationAsync(AuthorizationFilterContext context)\n",
                            validationErrors = (object)null
                        },
                        unAuthorizedRequest = true,
                        __abp = true
                    };

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                    return;
                }
            }

            await next(context);
        }

    }
}