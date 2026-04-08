using Div.Link.Project01.BLL.Dto;
using Div.Link.Project01.BLL.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Div.Link.Project01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorManager _doctorManager;

        public DoctorController(IDoctorManager doctorManager)
        {
            _doctorManager = doctorManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorReadDTO>>> GetAll()
        {
            var doctors = await _doctorManager.GetAllAsync();
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorReadDTO>> GetById(int id)
        {
            try
            {
                var doctor = await _doctorManager.GetByIdAsync(id);
                return Ok(doctor);
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] DoctorCreateDTO doctorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _doctorManager.CreateAsync(doctorDto);
            return Ok("Doctor created successfully");
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] DoctorUpdateDTO doctorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _doctorManager.UpdateAsync(doctorDto);
                return Ok("Doctor updated successfully");
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _doctorManager.DeleteByIdAsync(id);
                return Ok("Doctor deleted successfully");
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
