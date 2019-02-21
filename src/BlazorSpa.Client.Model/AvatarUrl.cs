using Newtonsoft.Json;

namespace BlazorSpa.Model {

	public class AvatarUrl {

		[JsonConstructor]
		public AvatarUrl(
			string url
		) {
			Url = url;
		}

		public string Url { get; }
	}
}
