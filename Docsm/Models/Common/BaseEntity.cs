namespace Docsm.Models.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedTime { get; set; }= DateTime.Now;
        public DateTime? UpdateTime { get; set; }
    }
}
