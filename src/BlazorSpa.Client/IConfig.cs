namespace BlazorSpa.Client {
	public interface IConfig {
		string Host { get; }

		string CongnitoUrl { get; }

		string TokenUrl { get; }

		string AuthUrl { get; }

		string CognitoClientId { get; }
	}
}
