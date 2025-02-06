using Docsm.Models.Common;

namespace Docsm.Models
{
    public class Specialty:BaseEntity 
    {
        public string Name { get; set; }
        public IEnumerable<Doctor>? Doctors { get; set; }
    }
}
