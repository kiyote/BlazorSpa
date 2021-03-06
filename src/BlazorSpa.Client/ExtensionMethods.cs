﻿/*
 * Copyright 2018-2019 Todd Lang
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using Microsoft.AspNetCore.Components.Services;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpa.Client {
	public static class ExtensionMethods {
		public static string GetParameter( this IUriHelper uriHelper, string name ) {
			var uri = new Uri( uriHelper.GetAbsoluteUri() );
			var value = QueryHelpers.ParseQuery( uri.Query ).TryGetValue( name, out var values ) ? values.First() : string.Empty;

			return value;
		}

		public static async Task<T> GetJsonAsync<T>( this HttpClient httpClient, string requestUri, Func<string, T> fromJson ) {
			var responseJson = await httpClient.GetStringAsync( requestUri );
			return fromJson( responseJson );
		}

		public static Task<S> PostJsonAsync<T, S>( this HttpClient httpClient, string requestUri, T content, Func<T, string> toJson, Func<string, S> fromJson )
			=> httpClient.SendJsonAsync<T, S>( HttpMethod.Post, requestUri, content, toJson, fromJson );

		public static Task PostJsonAsync<T>( this HttpClient httpClient, string requestUri, T content, Func<T, string> toJson )
			=> httpClient.SendJsonAsync<T, IgnoreResponse>( HttpMethod.Post, requestUri, content, toJson, default );

		public static Task<S> PutJsonAsync<T, S>( this HttpClient httpClient, string requestUri, T content, Func<T, string> toJson, Func<string, S> fromJson )
			=> httpClient.SendJsonAsync<T, S>( HttpMethod.Put, requestUri, content, toJson, fromJson );

		public static Task PutJsonAsync<T>( this HttpClient httpClient, string requestUri, T content, Func<T, string> toJson )
			=> httpClient.SendJsonAsync<T, IgnoreResponse>( HttpMethod.Put, requestUri, content, toJson, default );


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

		private class IgnoreResponse { }
	}
}
