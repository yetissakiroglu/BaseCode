using Economy.Core.Enums;
using Economy.Core.Models;
using Economy.Core.Tools;
using System.Net;
using System.Text.Json.Serialization;

namespace Economy.Persistence.Services
{


    public class ServiceResult : IResult
    {
        [JsonIgnore]
        public NotificationType Notification { get; set; }
        [JsonIgnore]
        public bool IsSuccess { get; set; }
        [JsonIgnore]
        public HttpStatusCode Status { get; set; }
        public ResultMessage Message { get; set; }
        public ResultValidationError ValidationError { get; set; }

        public ServiceResult()
        {
            IsSuccess = false;
        }

        // Başarısızlık durumları için statik metotlar
        public static IResult Fail(string message, HttpStatusCode code)
        {
            return new ServiceResult
            {
                IsSuccess = false,
                Message = new ResultMessage(message),
                Notification = NotificationType.Danger,
                Status = code
            };
        }

        public static IResult Fail(Dictionary<string, List<string>> validationError, HttpStatusCode code)
        {
            return new ServiceResult
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
            return new ServiceResult
            {
                IsSuccess = true,
                Notification = NotificationType.Success,
                Message = new ResultMessage(message),
                Status = code
            };
        }
    }

    public class ServiceResult<T> : ServiceResult, IResult<T>
    {
        public T Data { get; set; }

        // Başarısızlık durumları için statik metotlar
        public static ServiceResult<T> Fail(string message, HttpStatusCode code)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Message = new ResultMessage(message),
                Notification = NotificationType.Danger,
                Status = code
            };
        }
        public static ServiceResult<T> Fail(string message)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Message = new ResultMessage(message),
                Notification = NotificationType.Danger,
                Status = HttpStatusCode.NotFound
            };
        }
  
        // Başarı durumları için statik metotlar
        public static ServiceResult<T> Success(T data, HttpStatusCode code)
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                Data = data,
                Notification = NotificationType.Success,
                Message = new ResultMessage("İşlem Başarılı"),
                Status = code
            };
        }

        public static ServiceResult<T> Success(T data, string message, HttpStatusCode code)
        {
            return new ServiceResult<T>
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


