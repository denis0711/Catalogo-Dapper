using Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.AppData
{
    public static class Utils
    {
        public static Autenticacao ValidaToken(IEnumerable<Claim> claims, string chaveToken)
        {
            var tokenClaims = claims.Where(x => x.Type == nameof(Autenticacao));
            var autenticacao = new Autenticacao();
            try
            {
                if (claims == null || tokenClaims == null)
                    return null;

                foreach (var claim in tokenClaims)
                    autenticacao = JsonConvert.DeserializeObject<Autenticacao>(Infrastructure.CrossCutting.MD5.MD5.Decrypt(claim.Value, chaveToken));

                return autenticacao;
            }
            finally
            {
                tokenClaims = null;
            }
        }
    }
}
