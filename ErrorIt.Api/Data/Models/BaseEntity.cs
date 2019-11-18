using ErrorIt.Api.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErrorIt.Api.Data.Models
{
	public class BaseEntity : IBaseEntity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime UpdatedOn { get; set; }
		public bool Active { get; set; }
	}
}
