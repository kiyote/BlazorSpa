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

		async Task IUserApiService.RecordLogin() {
			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", await _accessTokenProvider.GetJwtToken() );
			var response = await _http.GetJsonAsync( $@"{_config.Host}/api/user/login",
				( s ) => { return JsonConvert.DeserializeObject<ApiUser>( s ); } );
		}

		async Task<ApiUser> IUserApiService.GetUserInformation() {
			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", await _accessTokenProvider.GetJwtToken() );
			var response = await _http.GetJsonAsync( $@"{_config.Host}/api/user",
				( s ) => { return JsonConvert.DeserializeObject<ApiUser>( s ); } );

			return response;
		}

		async Task<string> IUserApiService.SetAvatar( string contentType, string content ) {
			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", await _accessTokenProvider.GetJwtToken() );
			var request = new SetAvatarRequest() {
				ContentType = contentType,
				Content = content
			};
			var response = await _http.PostJsonAsync( $@"{_config.Host}/api/user/avatar", request,
				( r ) => { return JsonConvert.SerializeObject( r ); },
				( s ) => { return JsonConvert.DeserializeObject<SetAvatarResponse>(s); } );

			return response.Url;
		}
	}
}
