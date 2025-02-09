using Docsm.Models.Common;

namespace Docsm.Models
{
    public class Patient:BaseEntity 
    {
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
