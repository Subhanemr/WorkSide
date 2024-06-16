using Workwise.Application.Abstractions.Utilities;

namespace Workwise.Persistance.Utilities
{
    public class AlreadyExistException : Exception, IBaseException
    {
        public AlreadyExistException(string message = "This element is already exist") : base(message)
        {

        }
    }
}
