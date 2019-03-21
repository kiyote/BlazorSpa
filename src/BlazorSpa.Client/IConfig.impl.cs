namespace BlazorSpa.Client {
	internal sealed class Config : IConfig {
#if DEBUG
		private readonly string _host = "http://localhost:50738";
#else
		private static readonly string _host = "";
#endif

		private static readonly string _cognitoUrl = "https://blazorspa.auth.us-east-1.amazoncognito.com";

		private static readonly string _tokenUrl = $"{_cognitoUrl}/oauth2/token";

		private static readonly string _cognitoClientId = "5tghido5ese29dsktc40nd9s8o";

		private static readonly string _authUrl = $"{_cognitoUrl}/login?response_type=code&client_id={_cognitoClientId}";

		string IConfig.Host => _host;

		string IConfig.CongnitoUrl => _cognitoUrl;

		string IConfig.TokenUrl => _tokenUrl;

		string IConfig.AuthUrl => _authUrl;

		string IConfig.CognitoClientId => _cognitoClientId;
	}
}
