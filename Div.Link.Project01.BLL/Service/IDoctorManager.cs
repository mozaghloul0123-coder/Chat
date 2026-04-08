using Div.Link.Project01.BLL.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Div.Link.Project01.BLL.Service
{
    public interface IDoctorManager
    {
        Task<DoctorReadDTO> GetByIdAsync(int id);
        Task<IEnumerable<DoctorReadDTO>> GetAllAsync();
        Task DeleteByIdAsync(int id);
        Task UpdateAsync(DoctorUpdateDTO doctor);
        Task CreateAsync(DoctorCreateDTO doctor);
        Task SaveAsync();
    }
}