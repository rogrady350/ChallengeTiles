namespace ChallengeTiles.Server.Helpers
{
    public class ServiceResponse<T>
    {
        //wrapper for service responses
        public T Data { get; set; }         //response data
        public bool Success { get; set; }   //success flag
        public string Message { get; set; } //message to be sense
    }
}
