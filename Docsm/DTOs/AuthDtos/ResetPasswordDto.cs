using System.ComponentModel.DataAnnotations;

namespace Docsm.DTOs.AuthDtos
{
    public class ResetPasswordDto
    {
        public string Email {  get; set; }
        public int Code {  get; set; }
        [DataType (DataType.Password)]
        public string NewPassword {  get; set; }
    }
}
