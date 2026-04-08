using Div.Link.Project01.DAL.Models;
using System.Security.Claims;

namespace Div.Link.Project01.BLL.Service
{
    public interface IServicesAuth
    {
        //public Task<string> GetTokenAsync(string username, string password);

        public Task<string> GenerateToken(ApplicationUser user);
        public string GenerateRefreshToken();
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}