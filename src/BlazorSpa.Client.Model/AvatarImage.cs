using Newtonsoft.Json;

namespace BlazorSpa.Model {
	public class AvatarImage {

		[JsonConstructor]
		public AvatarImage(
			string contentType,
			string content
		) {
			ContentType = contentType;
			Content = content;
		}

		public string ContentType { get; }
		public string Content { get; }
	}
}
