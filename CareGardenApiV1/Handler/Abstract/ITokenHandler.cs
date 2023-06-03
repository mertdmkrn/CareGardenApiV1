using CareGardenApiV1.Handler.Model;
using System.Security.Claims;

namespace CareGardenApiV1.Handler.Abstract
{
    public interface ITokenHandler
    {
        Token CreateAccessToken(DateTime endDate, IList<Claim> claims);
    }
}
