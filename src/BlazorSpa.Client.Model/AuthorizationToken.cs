using Newtonsoft.Json;

namespace BlazorSpa.Client.Model {
	public class AuthorizationToken {

		[JsonConstructor]
		public AuthorizationToken(
			string id_token,
			string access_token,
			string token_type,
			int expires_in,
			string refresh_token
		) {
			this.id_token = id_token;
			this.access_token = access_token;
			this.token_type = token_type;
			this.expires_in = expires_in;
			this.refresh_token = refresh_token;
		}

		public string id_token { get; }
		public string access_token { get; }
		public string token_type { get; }
		public int expires_in { get; }
		public string refresh_token { get; }
	}
}
