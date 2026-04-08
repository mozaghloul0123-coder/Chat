using System;
using System.Collections.Generic;
using System.Text;

namespace Div.Link.Project01.BLL.Dto
{
    public class DoctorReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public string Phone { get; set; }
        public int ApplicationUserId { get; set; }
    }
}
