using Div.Link.Project01.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Div.Link.Project01.DAL.Repository
{
    public interface IDoctorRepository
    {
        public Task<Doctor> GetByIdAsync(int id);
        public Task<IEnumerable<Doctor>> GetAllAsync();
        public Task DeleteByIdAsync(int id);
        public Task UpdateAsync(Doctor doctor);
        public Task CreateAsync(Doctor doctor);
        public Task SaveAsync(); 
    }
}
