namespace BlazorSpa.Repository.Model {
	public sealed class AuthenticationUserInformation {

		public AuthenticationUserInformation(
			string authenticationId,
			string username,
			string email
		) {
			Username = username;
			Email = email;
			AuthenticationId = authenticationId;
		}

		public string Username { get; }

		public string Email { get; }

		public string AuthenticationId { get; }
	}
}
