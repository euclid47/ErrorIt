using System;

namespace ErrorIt.Api.Interfaces
{
	public interface IBaseEntity
	{
		DateTime CreatedOn { get; set; }
		int Id { get; set; }
		DateTime UpdatedOn { get; set; }
		bool Active { get; set; }
	}
}