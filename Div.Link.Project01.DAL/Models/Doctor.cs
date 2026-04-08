using System;
using System.Collections.Generic;
using System.Text;

namespace Div.Link.Project01.DAL.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; } //fk to ApplicationUser

        public ApplicationUser Application { get; set; }
    }
}

// test
