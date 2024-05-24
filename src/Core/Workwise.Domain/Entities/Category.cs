namespace Workwise.Domain.Entities
{
    public class Category : BaseNameEntity
    {
        public ICollection<Job>? Jobs { get; set; }
        public ICollection<Project>? Projects { get; set; }
    }
}
