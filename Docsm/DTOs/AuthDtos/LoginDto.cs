using System.ComponentModel;

namespace Docsm.DTOs.AuthDtos
{
    public class LoginDto
    {
        public string UsernameOrEmail {  get; set; }
        public string Password {  get; set; }
        [DefaultValue(false)]
        public bool RememberMe { get; set; } 
    }
}
