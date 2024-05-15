using CaptureIt.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CaptureIt.Middleware
{
    public class TokenValidation
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenValidation> _logger;

        public TokenValidation(RequestDelegate next, IConfiguration configuration, ILogger<TokenValidation> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {

            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (context.Request.Path.StartsWithSegments("/api/Authenticate/login"))
            {
                await _next(context);
                return;
            }

            if(context.Request.Path.StartsWithSegments("/api/Authenticate/register"))
            {
                await _next(context);
                return;
            }

            if (context.Request.Path.StartsWithSegments("/api/PasswordRecovery/forgot-password"))
            {
                await _next(context);
            }

            if (token == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unathorized: Missing token");
                return;
            }
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:Token"]));
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);



                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var username = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;

                context.Items["UserId"] = userId;
                context.Items["Username"] = username;

                await _next(context);
            }

            catch (Exception ex)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unathorized: Invalid token" + ex.Message);
                _logger.LogError(ex, "Error validating JWT token: {Message}", ex.Message);
            }

        }

    }
}
