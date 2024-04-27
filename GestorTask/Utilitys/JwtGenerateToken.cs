using GestorTask.Infraestructure.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GestorTask.Utilitys;

public static class JwtGenerateToken
{
    public static string GenerateToken(User UserInfo,string key, string issuer)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub,UserInfo.ToString()),
        };
        var token = new JwtSecurityToken(issuer, issuer, claims, expires: DateTime.Now.AddHours(1), signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);

    }
    //public static int? ValidateJwtToken(string token, string secret)
    //{
    //    if (token is null)
    //        return null;

    //    var tokenHandler = new JwtSecurityTokenHandler();
    //    var key = Encoding.ASCII.GetBytes(secret);
    //    try
    //    {
    //        tokenHandler.ValidateToken(token, new TokenValidationParameters
    //        {
    //            ValidateIssuerSigningKey = true,
    //            IssuerSigningKey = new SymmetricSecurityKey(key),
    //            ValidateIssuer = false,
    //            ValidateAudience = false,
    //            // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
    //            ClockSkew = TimeSpan.Zero
    //        }, out SecurityToken validatedToken);

    //        var jwtToken = (JwtSecurityToken)validatedToken;
    //        var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

    //        // return user id from JWT token if validation successful
    //        return userId;
    //    }
    //    catch
    //    {
    //        // return null if validation fails
    //        return null;
    //    }
    //}

}
