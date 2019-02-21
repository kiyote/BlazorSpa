using Microsoft.AspNetCore.Http;

namespace BlazorSpa.Server {
	internal sealed class ContextInformation: IContextInformation {

		private readonly IHttpContextAccessor _httpContextAccessor;

		public ContextInformation( IHttpContextAccessor httpContextAccessor) {
			_httpContextAccessor = httpContextAccessor;
		}

		public string Username {
			get {
				var context = _httpContextAccessor.HttpContext;
				return context.Items[ "User" ] as string;
			}
		}

		public string UserId {
			get {
				var context = _httpContextAccessor.HttpContext;
				return context.Items[ "UserId" ] as string;
			}
		}
	}
}
