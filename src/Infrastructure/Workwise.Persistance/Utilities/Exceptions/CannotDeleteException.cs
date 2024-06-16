using Workwise.Application.Abstractions.Utilities;

namespace Workwise.Persistance.Utilities
{
    public class CannotDeleteException : Exception, IBaseException
    {
        public CannotDeleteException(string message = "this object cannot be deleted") : base(message)
        {

        }
    }
}
