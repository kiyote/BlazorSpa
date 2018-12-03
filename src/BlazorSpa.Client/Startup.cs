using Blazor.Extensions.Logging;
using BlazorSpa.Client.Services;
using Cloudcrate.AspNetCore.Blazor.Browser.Storage;
using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlazorSpa.Client {
	public class Startup {
		public void ConfigureServices( IServiceCollection services ) {
			services.AddLogging( builder => builder
				 .AddBrowserConsole()
				 .SetMinimumLevel( LogLevel.Trace )
			);

			services.AddStorage();
			services.AddSingleton<AppState>();
			services.AddSingleton<IConfig, Config>();
			services.AddSingleton<ITokenService, TokenService>();
			services.AddSingleton<IAccessTokenProvider, AccessTokenProvider>();
			services.AddSingleton<ISignalService, SignalService>();
			services.AddSingleton<IUserApiService, UserApiService>();
		}

		public void Configure( IBlazorApplicationBuilder app ) {
			app.AddComponent<App>( "app" );
		}
	}
}
