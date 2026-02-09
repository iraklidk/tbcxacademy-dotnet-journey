namespace ATMApp
{
    internal class InvalidTransactionException : Exception
    {
        public InvalidTransactionException(string message) : base(message) { }
    }
}