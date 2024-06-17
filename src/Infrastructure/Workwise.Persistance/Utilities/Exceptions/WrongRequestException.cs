namespace Workwise.Persistance.Utilities
{
    public class WrongRequestException : Exception
    {
        public WrongRequestException(string message) : base(message) { }
    }
}
