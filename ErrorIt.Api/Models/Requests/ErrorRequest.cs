using System.ComponentModel.DataAnnotations;

namespace ErrorIt.Api.Models.Requests
{
	/// <summary>
	/// Error request model for simplified http get requests. Useful for applications
	/// needing a quick method to get an error message.
	/// </summary>
	public class ErrorRequest
	{
		[Required(AllowEmptyStrings = false)]
		public string ApplicationGroup { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string ApplicationName { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string ApplicationErrorCode { get; set; }
	}
}
