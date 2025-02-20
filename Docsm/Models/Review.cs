using Docsm.Models.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Docsm.Models
{
    public class Review:BaseEntity 
    {
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int? PatientId { get; set; }
        public Patient Patient { get; set; }
        [Range(1, 5)]
        public int? Rating { get; set; } 

        public string Comment { get; set; }

    }
}
