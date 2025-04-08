using Workwise.Application.Abstractions.Utilities;

namespace Workwise.Persistance.Utilities
{
    public class NotFoundException : Exception, IBaseException
    {
        public NotFoundException(string message = "Not found!") : base(message) { }
    }
}
