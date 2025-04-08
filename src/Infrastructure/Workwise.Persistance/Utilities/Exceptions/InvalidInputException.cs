using Workwise.Application.Abstractions.Utilities;

namespace Workwise.Persistance.Utilities
{
    public class InvalidInputException : Exception, IBaseException
    {
        public InvalidInputException(string message = "Invalid input!") : base(message) { }
    }
}
