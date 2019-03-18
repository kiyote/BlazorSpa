using Microsoft.AspNetCore.Components.Services;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Linq;

namespace BlazorSpa.Client {
	public static class ExtensionMethods {
		public static string GetParameter( this IUriHelper uriHelper, string name ) {
			var uri = new Uri( uriHelper.GetAbsoluteUri() );
			var value = QueryHelpers.ParseQuery( uri.Query ).TryGetValue( name, out var values ) ? values.First() : string.Empty;

			return value;
		}
	}
}
