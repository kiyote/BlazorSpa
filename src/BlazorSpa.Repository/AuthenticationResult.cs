using BlazorSpa.Model.Api;

namespace BlazorSpa.Repository {
	public class AuthenticationResult {

		public AuthenticationResult(
			AuthenticationStatus status,
			AuthorizationToken token = default,
			string session = default
		) {
			Status = status;
			Token = token;
			Session = session;
		}

		public AuthenticationStatus Status { get; }

		public AuthorizationToken Token { get; }

		public string Session { get; }
	}
}
