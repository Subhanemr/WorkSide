using Workwise.Application.Abstractions.Utilities;

namespace Workwise.Persistance.Utilities
{
    public class ConflictException : Exception, IBaseException
    {
        public ConflictException(string message = "Conflict!") : base(message) { }
    }
}
