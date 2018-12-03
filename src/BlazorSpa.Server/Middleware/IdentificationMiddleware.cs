using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace BlazorSpa.Server.Middleware {
	public class IdentificationMiddleware {

		private readonly RequestDelegate _next;

		public IdentificationMiddleware(
			RequestDelegate next
		) {
			_next = next;
		}

		public async Task InvokeAsync( HttpContext httpContext ) {
			var principal = httpContext.User?.Identities?.FirstOrDefault();

			if( ( principal?.Claims.Any() ?? false ) ) {
				var userIdValue = principal.Claims.FirstOrDefault( c => c.Type == ClaimTypes.NameIdentifier ).Value;
				var username = principal.Claims.FirstOrDefault( c => c.Type == "username" ).Value;

				httpContext.Items[ "User" ] = username;
			}

			await _next( httpContext );
		}
	}

	public static class IdentificationMiddlewareExtensions {
		public static IApplicationBuilder UseIdentificationMiddleware( this IApplicationBuilder builder ) {
			return builder.UseMiddleware<IdentificationMiddleware>();
		}
	}
}
