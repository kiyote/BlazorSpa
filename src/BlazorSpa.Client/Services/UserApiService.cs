using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BlazorSpa.Model.Api;
using Microsoft.AspNetCore.Blazor;

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

		async Task<UserInformationResponse> IUserApiService.GetUserInformation(UserInformationRequest request) {
			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", await _accessTokenProvider.GetJwtToken() );
			var response = await _http.GetJsonAsync<UserInformationResponse>( $@"{_config.Host}/api/user/{request.Username}" );

			return response;
		}
	}
}
