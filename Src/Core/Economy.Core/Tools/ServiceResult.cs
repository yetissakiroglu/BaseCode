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
		public ServiceResult()
		{
			IsSuccess =false;
		}
	
		public static IResult Fail(string message, HttpStatusCode code)
		{
			return new ServiceResult { IsSuccess = false, Message = new(message), Notification = NotificationType.Danger, Status = code };
		}

		public static IResult Fail(string message, bool isShow, HttpStatusCode code)
		{
			return new ServiceResult { IsSuccess = false, Message = new(message, isShow), Notification = NotificationType.Danger, Status = code };
		}

		public static IResult Fail(List<string> messages, HttpStatusCode code)
		{
			return new ServiceResult { IsSuccess = false, Message = new(messages), Notification = NotificationType.Danger, Status = code };
		}

		public static IResult Fail(List<string> messages, bool isShow, HttpStatusCode code)
		{
			return new ServiceResult { IsSuccess = false, Message = new(messages, isShow), Notification = NotificationType.Danger, Status = code };
		}

		//-------------------------------------------------------------

		public static IResult Success(string message, HttpStatusCode code)
		{
			return new ServiceResult { IsSuccess = true, Notification = NotificationType.Success, Message = new(message), Status = code };
		}
		public static IResult Success(string message, bool isShow, HttpStatusCode code)
		{
			return new ServiceResult { IsSuccess = true, Notification = NotificationType.Success, Message = new(message, isShow), Status = code };
		}

		public static IResult Success(List<string> messages, HttpStatusCode code)
		{
			return new ServiceResult { IsSuccess = true, Notification = NotificationType.Success, Message = new(messages), Status = code };
		}

		public static IResult Success(List<string> messages, bool isShow, HttpStatusCode code)
		{
			return new ServiceResult { IsSuccess = true, Notification = NotificationType.Success, Message = new(messages, isShow), Status = code };
		}
	}

	public class ServiceResult<T> : ServiceResult, IResult<T>
	{
		public ServiceResult()
		{
		}

		public T Data { get; set; }
		public static ServiceResult<T> Fail(string message)
		{
			return new ServiceResult<T> { IsSuccess = false, Message = new(message), Notification = NotificationType.Danger, Status = HttpStatusCode.NotFound };
		}

		public static ServiceResult<T> Fail(string message, HttpStatusCode code)
		{
			return new ServiceResult<T> { IsSuccess = false, Message = new(message), Notification = NotificationType.Danger, Status = code };
		}

		public static ServiceResult<T> Fail(string message, bool isShow, HttpStatusCode code)
		{
			return new ServiceResult<T> { IsSuccess = false, Message = new(message, isShow), Notification = NotificationType.Danger, Status = code };
		}

		public static ServiceResult<T> Fail(List<string> messages, HttpStatusCode code)
		{
			return new ServiceResult<T> { IsSuccess = false, Message = new(messages), Notification = NotificationType.Danger, Status = code };
		}

		public static ServiceResult<T> Fail(List<string> messages, bool isShow, HttpStatusCode code)
		{
			return new ServiceResult<T> { IsSuccess = false, Message = new(messages, isShow), Notification = NotificationType.Danger, Status = code };
		}

		// Success methods with data

		public static ServiceResult<T> Success(T data, HttpStatusCode code)
		{
			return new ServiceResult<T> { IsSuccess = true, Data = data, Notification = NotificationType.Success, Status = code };
		}


		public static ServiceResult<T> Success(T data, string message, HttpStatusCode code)
		{
			return new ServiceResult<T> { IsSuccess = true, Data = data, Notification = NotificationType.Success, Message = new(message), Status = code };
		}

		public static ServiceResult<T> Success(T data, string message, bool isShow, HttpStatusCode code)
		{
			return new ServiceResult<T> { IsSuccess = true, Data = data, Notification = NotificationType.Success, Message = new(message, isShow), Status = code };
		}

		public static ServiceResult<T> Success(T data, List<string> messages, HttpStatusCode code)
		{
			return new ServiceResult<T> { IsSuccess = true, Data = data, Notification = NotificationType.Success, Message = new(messages), Status = code };
		}

		public static ServiceResult<T> Success(T data, List<string> messages, bool isShow, HttpStatusCode code)
		{
			return new ServiceResult<T> { IsSuccess = true, Data = data, Notification = NotificationType.Success, Message = new(messages, isShow), Status = code };
		}

	}
}
