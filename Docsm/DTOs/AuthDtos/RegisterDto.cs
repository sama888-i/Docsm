using Docsm.Helpers.Enums;

namespace Docsm.DTOs.AuthDtos
{
    public class RegisterDto
    {
        public string UserName {  get; set; }
        public string Email {  get; set; }
        public string Name {  get; set; }
        public string Surname {  get; set; }
        public string Password {  get; set; }
        public DateOnly DateOfBirth {  get; set; }
        public Genders Gender { get; set; }
    }
}
