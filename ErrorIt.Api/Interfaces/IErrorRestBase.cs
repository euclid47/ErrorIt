using System.Collections.Generic;

namespace ErrorIt.Api.Interfaces
{
	public interface IErrorRestBase
	{
		string ApplicationErrorCode { get; set; }
		string Detail { get; set; }
		string Instance { get; set; }
		List<KeyValuePair<string, object>> Meta { get; set; }
		int Status { get; set; }
		string Title { get; set; }
		string Type { get; set; }
	}
}