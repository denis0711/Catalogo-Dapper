using System.Collections.Generic;
using System.Security.Claims;

namespace Infrastructure.CrossCutting.Jwt
{
    public interface IJwt
    {
        string CriarToken(List<Claim> claims);
    }
}
