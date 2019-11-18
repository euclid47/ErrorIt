using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErrorIt.Api.Data.Models
{
	public class Application : BaseEntity
	{
		public int ApplicationGroupId { get; set; }
		[MaxLength(255)]
		public string Name { get; set; }
		public string Description { get; set; }

		[ForeignKey("ApplicationGroupId")]
		public ApplicationGroup ApplicationGroup { get; set; }

		public List<ErrorTemplate> ErrorTemplates { get; set; }
	}
}
