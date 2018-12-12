using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BlazorSpa.Model;
using Newtonsoft.Json;

namespace BlazorSpa.Client.Services {
	internal sealed class UserApiService : IUserApiService {

		private readonly HttpClient _http;
		private readonly IAccessTokenProvider _accessTokenProvider;
		private readonly IConfig _config;

		public UserApiService(
			HttpClient http,
			IAccessTokenProvider accessTokenProvider,
			IConfig config
		) {
			_http = http;
			_accessTokenProvider = accessTokenProvider;
			_config = config;
		}

		async Task<User> IUserApiService.GetUserInformation() {
			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", await _accessTokenProvider.GetJwtToken() );
			var response = await _http.GetJsonAsync( $@"{_config.Host}/api/user",
				( s ) => { return JsonConvert.DeserializeObject<User>( s ); } );

			return response;
		}
	}
}
