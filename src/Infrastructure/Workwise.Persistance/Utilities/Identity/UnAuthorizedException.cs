using Workwise.Application.Abstractions.Utilities;

namespace Workwise.Persistance.Utilities
{
    public class UnAuthorizedException : Exception, IBaseException
    {
        public UnAuthorizedException(string message = "UnAuthorized 401!") : base(message)
        {

        }
    }
}
