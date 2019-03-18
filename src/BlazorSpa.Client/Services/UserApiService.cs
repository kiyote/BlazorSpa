using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BlazorSpa.Client.Model;
using BlazorSpa.Model;

namespace BlazorSpa.Client.Services {
	internal sealed class UserApiService : IUserApiService {

		private readonly HttpClient _http;
		private readonly IAccessTokenProvider _accessTokenProvider;
		private readonly IConfig _config;
		private readonly IJsonConverter _json;

		public UserApiService(
			HttpClient http,
			IAccessTokenProvider accessTokenProvider,
			IConfig config,
			IJsonConverter json
		) {
			_http = http;
			_accessTokenProvider = accessTokenProvider;
			_config = config;
			_json = json;
		}

		async Task IUserApiService.RecordLogin() {
			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", await _accessTokenProvider.GetJwtToken() );
			var response = await _http.GetJsonAsync( $@"{_config.Host}/api/user/login",
				( s ) => { return _json.Deserialize<User>( s ); } );
		}

		async Task<User> IUserApiService.GetUserInformation() {
			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", await _accessTokenProvider.GetJwtToken() );
			var response = await _http.GetJsonAsync( $@"{_config.Host}/api/user",
				( s ) => { return _json.Deserialize<User>( s ); } );

			return response;
		}

		async Task<string> IUserApiService.SetAvatar( string contentType, string content ) {
			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", await _accessTokenProvider.GetJwtToken() );
			var request = new AvatarImage(
				contentType,
				content
			);
			var response = await _http.PostJsonAsync( $@"{_config.Host}/api/user/avatar", request,
				( r ) => { return _json.Serialize( r ); },
				( s ) => { return _json.Deserialize<AvatarUrl>( s ); } );

			return response.Url;
		}
	}
}
