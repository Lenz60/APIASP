using API.Helper.Interfaces;
using API.ViewModel;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

namespace API.Helper
{
    public class JWTHelper : IJWTHelper
    {
        private readonly IConfiguration _configuration;

        public JWTHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(CredsPayload payload)
        {
            var secret = _configuration.GetConnectionString("Secret");
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(payload, secret);
            return token;
        }
    }
}
