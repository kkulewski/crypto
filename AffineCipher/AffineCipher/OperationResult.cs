namespace AffineCipher
{
    public class OperationResult
    {
        public bool Success;

        public string Message;

        public OperationResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
