namespace BlogService.Common.Utilities
{
    public class ResponseWrapper
    {
        public ResponseWrapper()
        {

        }
        public ResponseWrapper(object responseData, string message, int statusCode, bool isSuccess)
        {
            data = responseData;
            Message = message;
            StatusCode = statusCode;
            IsSuccess = isSuccess;
        }

        public object data { get; set; }
        public object Message { get; set; }
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
    }
}
