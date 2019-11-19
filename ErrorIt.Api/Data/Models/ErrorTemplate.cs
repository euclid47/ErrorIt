using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErrorIt.Api.Data.Models
{
	public class ErrorTemplate : BaseEntity
	{
		public int ApplicationId { get; set; }
		[MaxLength(50)]
		public string ApplicationErrorCode { get; set; }
		public string ErrorDetail { get; set; }

		[ForeignKey("ApplicationId")]
		public Application Application { get; set; }
	}
}
