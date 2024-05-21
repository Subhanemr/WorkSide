using System.Reflection.Metadata.Ecma335;

namespace Workwise.Domain.Entities
{
    public class Category : BaseNameEntity
    {
        public List<Job>? Jobs { get; set; }
        public List<Project>? Projects { get; set; }
    }
}
