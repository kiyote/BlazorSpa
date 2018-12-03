using System;
using System.Linq;
using Microsoft.AspNetCore.Blazor.Services;
using Microsoft.AspNetCore.WebUtilities;

namespace BlazorSpa.Client {
	public static class ExtensionMethods {
		public static string GetParameter(this IUriHelper uriHelper, string name) {
			var uri = new Uri( uriHelper.GetAbsoluteUri() );
			var value = QueryHelpers.ParseQuery( uri.Query ).TryGetValue( name, out var values ) ? values.First() : string.Empty;

			return value;
		}
	}
}
