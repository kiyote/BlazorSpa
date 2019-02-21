﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorSpa.Client.Model;
using BlazorSpa.Client.Pages.Auth;
using Newtonsoft.Json;

namespace BlazorSpa.Client.Services {
	public class TokenService : ITokenService {

		private readonly HttpClient _http;
		private readonly IConfig _config;
		private readonly JsonSerializerSettings _settings;

		public TokenService(
			HttpClient httpClient,
			IConfig config
		) {
			_http = httpClient;
			_config = config;

			_settings = new JsonSerializerSettings() {
				DateParseHandling = DateParseHandling.None,
				DateTimeZoneHandling = DateTimeZoneHandling.Utc,
				DateFormatHandling = DateFormatHandling.IsoDateFormat
			};
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
				var tokens = JsonConvert.DeserializeObject<AuthorizationToken>( payload, _settings );

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
				var tokens = JsonConvert.DeserializeObject<AuthorizationToken>( payload, _settings );

				return tokens;
			}

			return default;
		}
	}
}
