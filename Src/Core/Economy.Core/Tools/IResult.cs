using Economy.Core.Enums;
using Economy.Core.Models;
using System.Net;
using System.Text.Json.Serialization;

namespace Economy.Core.Tools
{
	public interface IResult
	{
		ResultMessage Message { get; set; }

		[JsonIgnore]
		bool IsSuccess { get; set; }
		[JsonIgnore]
		HttpStatusCode Status { get; set; }
		[JsonIgnore]
		NotificationType Notification { get; }
	}

	public interface IResult<out T> : IResult
	{
		T Data { get; }
	}
}
