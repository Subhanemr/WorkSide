namespace Workwise.Domain.Entities
{
    public abstract class BaseEntity
    {
        public string Id { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string CreatedBy { get; set; } = null!;
    }
}
