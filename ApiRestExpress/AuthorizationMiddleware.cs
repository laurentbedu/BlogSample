using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ApiRestExpress
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public AuthorizationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public Task Invoke(HttpContext httpContext)
        {
            string path = httpContext.Request.Path.ToString().Replace("/api/", "");
            string method = httpContext.Request.Method;
            var isAnonymous = _configuration.GetValue<bool>($"Routing:AllowAnonymous:{method}:{path}");
            if (!isAnonymous)
            {
                var tokenParams = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]))
                };
                httpContext.Request.Headers.TryGetValue("Authorization", out var token);
                var jwt = token.ToString().Replace("Bearer ", "");


                try
                {
                    new JwtSecurityTokenHandler().ValidateToken(jwt, tokenParams, out var validated);
                    //var tok = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
                    //var exp = tok.Claims.First(claim => claim.Type == "exp").Value;
                    //if (DateTime.MinValue.AddSeconds(Int32.Parse(exp)) > DateTime.Now)
                    //{

                    //}
                    if (validated != null)
                    {
                        return _next(httpContext);
                    }
                }
                catch (Exception ex)
                {
                }
                httpContext.Response.StatusCode = 301;
                return Task.CompletedTask;
            }
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationMiddleware>();
        }
    }
}
