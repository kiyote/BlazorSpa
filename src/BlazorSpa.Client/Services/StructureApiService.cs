using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BlazorSpa.Model;
using Newtonsoft.Json;

namespace BlazorSpa.Client.Services {
	internal sealed class StructureApiService : IStructureApiService {

		private readonly HttpClient _http;
		private readonly IAccessTokenProvider _accessTokenProvider;
		private readonly IConfig _config;

		public StructureApiService(
			HttpClient http,
			IAccessTokenProvider accessTokenProvider,
			IConfig config
		) {
			_http = http;
			_accessTokenProvider = accessTokenProvider;
			_config = config;
		}

		async Task<IEnumerable<ApiView>> IStructureApiService.GetViews() {
			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", await _accessTokenProvider.GetJwtToken() );
			var response = await _http.GetJsonAsync( $@"{_config.Host}/api/structure/views",
				( s ) => { return JsonConvert.DeserializeObject<ApiView[]>( s ); } );

			return response;
		}
	}
}
