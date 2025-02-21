using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Docsm.DTOs.AuthDtos
{
    public class LoginDto
    {
        public string UsernameOrEmail {  get; set; }
        [DataType (DataType.Password)]
        public string Password {  get; set; }
        [DefaultValue(false)]
        public bool RememberMe { get; set; } 
    }
}
