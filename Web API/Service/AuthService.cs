using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Web_API.Service;

public class AuthService(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    //creating a token in a modern way .net 9 >=
    public string CreateToken(List<Claim> claims, DateTime expiresAtSeconds)
    {
        //in a old fashioned way you can use security token handler in the derived type JwtSecurityTokenHandler
        //modern way is use json web token handler: .net 9 >= has better performance
        var claimsDict = claims is not null && claims.Count == 0 ? [] : claims.ToDictionary(x => x.Type, x => (object)x.Value);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = new(
                new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["SecretKey"] ?? string.Empty)), //in user secrets 
                SecurityAlgorithms.HmacSha256Signature
            ),
            Claims = claimsDict,
            Expires = expiresAtSeconds,
        };

        var tokenHandler = new JsonWebTokenHandler();
        return tokenHandler.CreateToken(tokenDescriptor);
    }

    public string CreateTokenOldWay(List<Claim> claims, DateTime expiresAtSeconds)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"] ?? string.Empty));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Aqui, diferente do JsonWebTokenHandler, não é preciso converter claims em dicionário.
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAtSeconds,
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
