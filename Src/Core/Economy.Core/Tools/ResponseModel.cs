using Economy.Core.Enums;
using Economy.Core.Tools.Models;
using System.Net;
using System.Text.Json.Serialization;

namespace Economy.Core.Tools
{
    public class ResponseModel(bool isSuccess = false) : IResult
    {
        [JsonIgnore]
        public NotificationType Notification { get; set; }

        [JsonIgnore]
        public bool IsSuccess { get; set; } = isSuccess;

        [JsonIgnore]
        public HttpStatusCode Status { get; set; }

        public ResultMessage Message { get; set; }

        public ResultValidationError ValidationError { get; set; }

        // Başarısızlık durumları için statik metotlar
        public static IResult Fail(string message, HttpStatusCode code)
        {
            return new ResponseModel
            {
                IsSuccess = false,
                Message = new ResultMessage(message),
                Notification = NotificationType.Danger,
                Status = code
            };
        }

        public static IResult Fail(Dictionary<string, List<string>> validationError, HttpStatusCode code)
        {
            return new ResponseModel
            {
                IsSuccess = false,
                ValidationError = new ResultValidationError(validationError),
                Notification = NotificationType.Danger,
                Status = code
            };
        }

        // Başarı durumları için statik metotlar
        public static IResult Success(string message, HttpStatusCode code)
        {
            return new ResponseModel
            {
                IsSuccess = true,
                Message = new ResultMessage(message),
                Notification = NotificationType.Success,
                Status = code
            };
        }
    }

    public class ResponseModel<T> : ResponseModel, IResult<T>
    {
        public T Data { get; set; } = default!;

        public ResponseModel(bool isSuccess = false) : base(isSuccess) { }

        // Başarısızlık durumları için statik metotlar
        public static new ResponseModel<T> Fail(string message, HttpStatusCode code)
        {
            return new ResponseModel<T>
            {
                IsSuccess = false,
                Message = new ResultMessage(message),
                Notification = NotificationType.Danger,
                Status = code
            };
        }

        public static ResponseModel<T> Fail(string message)
        {
            return new ResponseModel<T>
            {
                IsSuccess = false,
                Message = new ResultMessage(message),
                Notification = NotificationType.Danger,
                Status = HttpStatusCode.NotFound
            };
        }

        // Başarı durumları için statik metotlar
        public static ResponseModel<T> Success(T data, HttpStatusCode code)
        {
            return new ResponseModel<T>
            {
                IsSuccess = true,
                Data = data,
                Notification = NotificationType.Success,
                Message = new ResultMessage("İşlem Başarılı"),
                Status = code
            };
        }

        public static ResponseModel<T> Success(T data, string message, HttpStatusCode code)
        {
            return new ResponseModel<T>
            {
                IsSuccess = true,
                Data = data,
                Notification = NotificationType.Success,
                Message = new ResultMessage(message),
                Status = code
            };
        }
    }

}
