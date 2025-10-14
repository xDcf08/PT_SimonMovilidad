using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimonMovilidad.Persistence.Authentication;
using System.Text;

namespace SimonMovilidad.API.OptionsSetup
{
    public class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtOptions _options;

        public JwtBearerOptionsSetup(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            ConfigureOptions(options);
        }

        public void Configure(JwtBearerOptions options)
        {
            ConfigureOptions(options);
        }

        private void ConfigureOptions(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _options.Issuer,
                ValidAudience = _options.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey!))
            };
        }
    }
}
