using Newtonsoft.Json;

namespace ErrorIt.Api.Extensions
{
	public static class ObjectExtensions
	{
		public static T DeserializeJson<T>(this string val)
		{
			return string.IsNullOrWhiteSpace(val)
				? default(T)
				: JsonConvert.DeserializeObject<T>(val);
		}

		public static string SerializeJson(this object val, bool prettyPrint = false)
		{
			return JsonConvert.SerializeObject(val, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = prettyPrint ? Formatting.Indented : Formatting.None });
		}
	}
}
