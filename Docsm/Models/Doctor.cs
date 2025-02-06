using Docsm.Models.Common;

namespace Docsm.Models
{
    public class Doctor:BaseEntity 
    {
        public string UserId {  get; set; }
        public User User { get; set; }
        public int SpecialtyId {  get; set; }
        public Specialty Specialty { get; set; }
        public string Adress {  get; set; } 
        public double Latitude {  get; set; }
        public double Longitude { get; set; }


    }
}
