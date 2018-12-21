using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorSpa.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace BlazorSpa.Server.Middleware {
	public class IdentificationMiddleware {

		private readonly RequestDelegate _next;
		private readonly IUserRepository _userRepository;

		public IdentificationMiddleware(
			RequestDelegate next,
			IUserRepository userRepository
		) {
			_next = next;
			_userRepository = userRepository;
		}

		public async Task InvokeAsync( HttpContext httpContext ) {
			var principal = httpContext.User?.Identities?.FirstOrDefault();

			if( ( principal?.Claims.Any() ?? false ) ) {
				var userIdValue = principal.Claims.FirstOrDefault( c => c.Type == ClaimTypes.NameIdentifier ).Value;
				var username = principal.Claims.FirstOrDefault( c => c.Type == "username" ).Value;

				httpContext.Items[ "User" ] = username;

				var user = await _userRepository.GetByUsername( username );
				if (user != default) {
					httpContext.Items[ "UserId" ] = user.Id.Value;
				}
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
