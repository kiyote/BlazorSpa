/*
 * Copyright 2018-2019 Todd Lang
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
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
