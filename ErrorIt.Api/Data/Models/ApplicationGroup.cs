using System.ComponentModel.DataAnnotations;

namespace ErrorIt.Api.Data.Models
{
	public class ApplicationGroup : BaseEntity
	{
		[MaxLength(50)]
		public string Name { get; set; }
		public string Description { get; set; }
	}
}
