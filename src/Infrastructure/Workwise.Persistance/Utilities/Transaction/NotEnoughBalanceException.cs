using Workwise.Application.Abstractions.Utilities;

namespace Workwise.Persistance.Utilities
{
    public class NotEnoughBalanceException : Exception, IBaseException
    {
        public NotEnoughBalanceException(string message = "there is not enough balance") : base(message)
        {

        }
    }
}
