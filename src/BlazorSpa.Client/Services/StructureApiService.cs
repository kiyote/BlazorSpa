using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BlazorSpa.Client.Model;
using BlazorSpa.Shared;

namespace BlazorSpa.Client.Services {
	internal sealed class StructureApiService : IStructureApiService {

		private const string StructureApiUrl = "/api/structure";
		private readonly HttpClient _http;
		private readonly IAccessTokenProvider _accessTokenProvider;
		private readonly IConfig _config;
		private readonly IJsonConverter _json;

		public StructureApiService(
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

		async Task<IEnumerable<View>> IStructureApiService.GetAllViews() {
			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", await _accessTokenProvider.GetJwtToken() );
			var response = await _http.GetJsonAsync( $@"{_config.Host}{StructureApiUrl}/views",
				( s ) => { return _json.Deserialize<View[]>( s ); } );

			return response;
		}

		async Task<IEnumerable<View>> IStructureApiService.GetUserViews() {
			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", await _accessTokenProvider.GetJwtToken() );
			var response = await _http.GetJsonAsync( $@"{_config.Host}{StructureApiUrl}/view",
				( s ) => { return _json.Deserialize<View[]>( s ); } );

			return response;
		}

		async Task<View> IStructureApiService.CreateView( 
			string viewType, 
			string viewName 
		) {
			if (string.IsNullOrWhiteSpace(viewType)
				|| string.IsNullOrWhiteSpace(viewName)) {
				return default;
			}

			var newView = new View( default, viewType, viewName );

			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", await _accessTokenProvider.GetJwtToken() );
			var response = await _http.PostJsonAsync( $@"{_config.Host}{StructureApiUrl}/view",
				newView,
				( v ) => { return _json.Serialize( v ); },
				( s ) => { return _json.Deserialize<View>( s ); } );

			return response;
		}

		async Task<Structure> IStructureApiService.CreateStructureInView( Id<View> viewId, string structureType, string name ) {
			if (viewId == default
				|| string.IsNullOrWhiteSpace(structureType)
				|| string.IsNullOrWhiteSpace(name)) {
				return default;
			}

			var newStructure = new Structure( default, structureType, name );
			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", await _accessTokenProvider.GetJwtToken() );
			var response = await _http.PostJsonAsync( $@"{_config.Host}{StructureApiUrl}/view/{viewId.Value}",
				newStructure,
				( v ) => { return _json.Serialize( v ); },
				( s ) => { return _json.Deserialize<Structure>( s ); } );

			return response;
		}

		async Task<ApiStructureOperation> IStructureApiService.AddStructureToView(Id<Structure> structureId, Id<View> viewId) {
			if (structureId == default 
				|| viewId == default) {
				return default;
			}

			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", await _accessTokenProvider.GetJwtToken() );
			var response = await _http.PostJsonAsync( $@"{_config.Host}{StructureApiUrl}/{structureId}/view/{viewId}",
				default(Structure),
				( v ) => { return _json.Serialize(default); },
				( s ) => { return _json.Deserialize<ApiStructureOperation>( s ); } );

			return response;
		}

		async Task<IEnumerable<Structure>> IStructureApiService.GetViewStructures(Id<View> viewId) {
			if (viewId == default) {
				return default;
			}

			_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", await _accessTokenProvider.GetJwtToken() );
			var response = await _http.GetJsonAsync( $@"{_config.Host}{StructureApiUrl}/view/{viewId.Value}/structures",
				( s ) => { return _json.Deserialize<Structure[]>( s ); } );

			return response;
		}
	}
}
