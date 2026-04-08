using AutoMapper;
using Div.Link.Project01.BLL.Dto;
using Div.Link.Project01.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Div.Link.Project01.BLL.AutoMapper
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            // Create
            CreateMap<DoctorCreateDTO, Doctor>();

            // Update
            CreateMap<DoctorUpdateDTO, Doctor>();

            // Read
            CreateMap<Doctor, DoctorReadDTO>();
        
        }   
    }
}
