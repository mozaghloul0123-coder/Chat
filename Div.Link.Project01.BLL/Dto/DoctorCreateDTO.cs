using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Div.Link.Project01.BLL.Dto
{


    public class DoctorCreateDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Specialization { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required]
        public int ApplicationId { get; set; }

    }
}