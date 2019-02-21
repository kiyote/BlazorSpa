using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BlazorSpa.Client.Model;
using Newtonsoft.Json;

namespace BlazorSpa.Client.Services {
	internal sealed class StructureApiService : IStructureApiService {

		private const string StructureApiUrl = "/api/structure";
		private readonly HttpClient _http;
		private readonly IAccessTokenProvider _accessTokenProvider;
		private readonly IConfig _config;
		private readonly JsonSerializerSettings _settings;

		public StructureApiService(
			HttpClient http,
			IAccessTokenProvider accessTokenProvider,
			IConfig config
		) {
			_http = http;
			_accessTokenProvider = accessTokenProvider;
			_config = config;

			_settings = new JsonSerializerSettings() {
				DateParseHandling = DateParseHandling.None,
				DateTimeZoneHandling = DateTimeZoneHandling.Utc,
				DateFormatHandling = DateFormatHandling.IsoDateFormat
			};

		}

		async Task<IEnumerable<View>> IStructureApiService.GetAllViews() {
			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", await _accessTokenProvider.GetJwtToken() );
			var response = await _http.GetJsonAsync( $@"{_config.Host}{StructureApiUrl}/views",
				( s ) => { return JsonConvert.DeserializeObject<View[]>( s, _settings ); } );

			return response;
		}

		async Task<IEnumerable<View>> IStructureApiService.GetUserViews() {
			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", await _accessTokenProvider.GetJwtToken() );
			var response = await _http.GetJsonAsync( $@"{_config.Host}{StructureApiUrl}/view",
				( s ) => { Console.WriteLine( s ); return JsonConvert.DeserializeObject<View[]>( s, _settings ); } );

			return response;
		}

		async Task<View> IStructureApiService.CreateView( 
			string viewType, 
			string viewName 
		) {
			var newView = new View( default, viewType, viewName );

			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", await _accessTokenProvider.GetJwtToken() );
			var response = await _http.PostJsonAsync( $@"{_config.Host}{StructureApiUrl}/view",
				newView,
				( v ) => { return JsonConvert.SerializeObject( v ); },
				( s ) => { return JsonConvert.DeserializeObject<View>( s, _settings ); } );

			return response;
		}

		async Task<Structure> IStructureApiService.CreateStructureInView( string viewId, string structureType ) {
			var newStructure = new Structure( default, structureType );
			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", await _accessTokenProvider.GetJwtToken() );
			var response = await _http.PostJsonAsync( $@"{_config.Host}{StructureApiUrl}/view/{viewId}",
				newStructure,
				( v ) => { return JsonConvert.SerializeObject( v ); },
				( s ) => { return JsonConvert.DeserializeObject<Structure>( s, _settings ); } );

			return response;
		}

		async Task<ApiStructureOperation> IStructureApiService.AddStructureToView(string structureId, string viewId) {
			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", await _accessTokenProvider.GetJwtToken() );
			var response = await _http.PostJsonAsync( $@"{_config.Host}{StructureApiUrl}/{structureId}/view/{viewId}",
				default(Structure),
				( v ) => { return "{}"; },
				( s ) => { return JsonConvert.DeserializeObject<ApiStructureOperation>( s, _settings ); } );

			return response;
		}
	}
}
