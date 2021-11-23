using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.CrossCutting.Jwt
{
    public class Jwt : IJwt
    {
        private readonly string _chave;
        private readonly TimeSpan _tempoVidaToken;

        public Jwt(string chave, TimeSpan tempoVidaToken)
        {
            this._chave = chave;
            this._tempoVidaToken = tempoVidaToken;
        }

        public string CriarToken(List<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(this._chave);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(this._tempoVidaToken),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
