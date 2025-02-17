using Docsm.Helpers.Enums;
using Microsoft.AspNetCore.Identity;

namespace Docsm.Models
{
    public class User:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? ProfileImageUrl { get; set; } 
        public Genders  Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
