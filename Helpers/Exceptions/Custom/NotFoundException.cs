namespace Api.Helpers.Exceptions.Custom
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) {}
    }
}
