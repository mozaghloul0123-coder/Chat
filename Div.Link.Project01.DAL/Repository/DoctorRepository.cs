using Div.Link.Project01.DAL.Data;
using Div.Link.Project01.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Div.Link.Project01.DAL.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext dbContext;

        public DoctorRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Create
        public async Task CreateAsync(Doctor doctor)
        {
            if (doctor == null)
                throw new ArgumentNullException(nameof(doctor));

            await dbContext.Doctors.AddAsync(doctor);
        }

        // Delete
        public async Task DeleteByIdAsync(int id)
        {
            var doctor = await dbContext.Doctors.FindAsync(id);
            if (doctor == null)
                throw new ArgumentNullException(nameof(doctor));

            dbContext.Doctors.Remove(doctor);
        }

        // Get All
        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await dbContext.Doctors.ToListAsync();
        }

        // Get By Id
        public async Task<Doctor> GetByIdAsync(int id)
        {
            var doctor = await dbContext.Doctors.FindAsync(id);
            if (doctor == null)
                throw new ArgumentNullException(nameof(doctor));

            return doctor;
        }

        // Save (Commit)
        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        // Update
        public Task UpdateAsync(Doctor doctor)
        {
            if (doctor == null)
                throw new ArgumentNullException(nameof(doctor));
      
            return Task.CompletedTask;
        }
    }
}