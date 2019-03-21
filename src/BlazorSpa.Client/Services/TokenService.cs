using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorSpa.Client.Model;
using BlazorSpa.Client.Pages.Auth;

namespace BlazorSpa.Client.Services {
	internal sealed class TokenService : ITokenService {

		private readonly HttpClient _http;
		private readonly IConfig _config;
		private readonly IJsonConverter _json;

		public TokenService(
			HttpClient httpClient,
			IConfig config,
			IJsonConverter json
		) {
			_http = httpClient;
			_config = config;
			_json = json;
		}

		async Task<AuthorizationToken> ITokenService.GetToken( string code ) {
			var redirectUrl = _config.Host + ValidateComponent.Url;

			var content = new FormUrlEncodedContent( new List<KeyValuePair<string, string>>() {
				new KeyValuePair<string, string>("grant_type", "authorization_code"),
				new KeyValuePair<string, string>("client_id", _config.CognitoClientId),
				new KeyValuePair<string, string>("code", code),
				new KeyValuePair<string, string>("redirect_uri", redirectUrl)
			} );
			var response = await _http.PostAsync( _config.TokenUrl, content );
			if( response.IsSuccessStatusCode ) {
				var payload = await response.Content.ReadAsStringAsync();
				var tokens = _json.Deserialize<AuthorizationToken>( payload );

				return tokens;
			}

			return default;
		}

		async Task<AuthorizationToken> ITokenService.RefreshToken( string refreshToken ) {
			var content = new FormUrlEncodedContent( new List<KeyValuePair<string, string>>() {
				new KeyValuePair<string, string>("grant_type", "refresh_token"),
				new KeyValuePair<string, string>("client_id", _config.CognitoClientId),
				new KeyValuePair<string, string>("refresh_token", refreshToken)
			} );
			var response = await _http.PostAsync( _config.TokenUrl, content );
			if( response.IsSuccessStatusCode ) {
				var payload = await response.Content.ReadAsStringAsync();
				var tokens = _json.Deserialize<AuthorizationToken>( payload );

				return tokens;
			}

			return default;
		}
	}
}
