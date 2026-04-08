using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Div.Link.Project01.DAL.Models
{
    public class ApplicationUser :IdentityUser
    {
        public Doctor Doctor { get; set; }
        public int? DoctorId { get; set; }
        public string? Address { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
