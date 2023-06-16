namespace TMS.API.DTOs
{
    public class ResponseDTO<T>
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public T Response { get; set; }
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
