using AutoMapper;
using Div.Link.Project01.BLL.Dto;
using Div.Link.Project01.DAL.Models;
using Div.Link.Project01.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Div.Link.Project01.BLL.Service
{
    public class DoctorManager : IDoctorManager
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public DoctorManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        // Create
        public async Task CreateAsync(DoctorCreateDTO dto)
        {
            var doctor = mapper.Map<Doctor>(dto);
            await unitOfWork.DoctorRepository.CreateAsync(doctor);
            await unitOfWork.SaveChangesAsync();
        }

        // Get all
        public async Task<IEnumerable<DoctorReadDTO>> GetAllAsync()
        {
            var doctors = await unitOfWork.DoctorRepository.GetAllAsync();
            return mapper.Map<IEnumerable<DoctorReadDTO>>(doctors);
        }

        // Get by Id
        public async Task<DoctorReadDTO> GetByIdAsync(int id)
        {
            var doctor = await unitOfWork.DoctorRepository.GetByIdAsync(id);
            if (doctor == null)
                throw new Exception("Doctor not found");
            return mapper.Map<DoctorReadDTO>(doctor);
        }

        // Update
        public async Task UpdateAsync(DoctorUpdateDTO dto)
        {
            var doctor = await unitOfWork.DoctorRepository.GetByIdAsync(dto.Id);
            if (doctor == null)
                throw new Exception("Doctor not found");

            mapper.Map(dto, doctor);
            await unitOfWork.SaveChangesAsync();
        }

        // Delete
        public async Task DeleteByIdAsync(int id)
        {
            var doctor = await unitOfWork.DoctorRepository.GetByIdAsync(id);
            if (doctor == null)
                throw new Exception("Doctor not found");

            await unitOfWork.DoctorRepository.DeleteByIdAsync(doctor.Id);
            await unitOfWork.SaveChangesAsync();
        }

        // Save (Optional if using UnitOfWork)
        public async Task SaveAsync()
        {
            await unitOfWork.SaveChangesAsync();
        }
    }
}