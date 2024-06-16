using Workwise.Application.Abstractions.Utilities;

namespace Workwise.Persistance.Utilities
{
    public class LoginException : Exception, IBaseException
    {
        public LoginException(string message = "Email or password is wrong!") : base(message)
        {

        }
    }
}
