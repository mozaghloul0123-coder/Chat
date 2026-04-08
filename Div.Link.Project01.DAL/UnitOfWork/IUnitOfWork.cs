using Div.Link.Project01.DAL.Repository;

namespace Div.Link.Project01.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        public void Dispose();
        public Task<int> SaveChangesAsync();    
        public IDoctorRepository DoctorRepository { get; }
    }
}