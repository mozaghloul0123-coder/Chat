using Div.Link.Project01.DAL.Data;
using Div.Link.Project01.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Div.Link.Project01.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;
        private IDoctorRepository _doctorRepository;
        public UnitOfWork(ApplicationDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public IDoctorRepository DoctorRepository => _doctorRepository ??= new DoctorRepository(dbContext);

        public void Dispose() 
        {
                dbContext.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
         return await   dbContext.SaveChangesAsync();
        }
    }
        
    
}
