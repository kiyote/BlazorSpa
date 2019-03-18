using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpa.Client {
	/// <summary>
	/// Extension methods for working with JSON APIs.
	/// </summary>
	public static class HttpClientJsonExtensions {
		public static async Task<T> GetJsonAsync<T>( this HttpClient httpClient, string requestUri, Func<string, T> fromJson ) {
			var responseJson = await httpClient.GetStringAsync( requestUri );
			return fromJson( responseJson );
		}

		public static Task<S> PostJsonAsync<T, S>( this HttpClient httpClient, string requestUri, T content, Func<T, string> toJson, Func<string, S> fromJson )
			=> httpClient.SendJsonAsync<T, S>( HttpMethod.Post, requestUri, content, toJson, fromJson );

		public static Task<S> PutJsonAsync<T, S>( this HttpClient httpClient, string requestUri, T content, Func<T, string> toJson, Func<string, S> fromJson )
			=> httpClient.SendJsonAsync<T, S>( HttpMethod.Put, requestUri, content, toJson, fromJson );


		public static async Task<S> SendJsonAsync<T, S>( this HttpClient httpClient, HttpMethod method, string requestUri, T content, Func<T, string> toJson, Func<string, S> fromJson ) {
			var requestJson = toJson( content );
			var response = await httpClient.SendAsync( new HttpRequestMessage( method, requestUri ) {
				Content = new StringContent( requestJson, Encoding.UTF8, "application/json" )
			} );

			if( typeof( S ) == typeof( IgnoreResponse ) ) {
				return default;
			} else {
				var responseJson = await response.Content.ReadAsStringAsync();
				return fromJson( responseJson );
			}
		}

		class IgnoreResponse { }
	}
}
