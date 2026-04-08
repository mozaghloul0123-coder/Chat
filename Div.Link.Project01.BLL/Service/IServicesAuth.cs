using Div.Link.Project01.DAL.Models;

namespace Div.Link.Project01.BLL.Service
{
    public interface IServicesAuth
    {
        //public Task<string> GetTokenAsync(string username, string password);

        public Task<string> GenerateToken(ApplicationUser user);
    }
}