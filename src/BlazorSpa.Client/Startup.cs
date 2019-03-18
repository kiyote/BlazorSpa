using BlazorSpa.Client.Services;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlazorSpa.Client {
	public class Startup {
		public void ConfigureServices( IServiceCollection services ) {
			services.AddLogging( builder => builder
				 .SetMinimumLevel( LogLevel.Trace )
			);

			services.AddSingleton<IJsonConverter, JsonConverter>();
			services.AddSingleton<IConfig, Config>();

			services.AddScoped<AppState>();
			services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<IAccessTokenProvider, AccessTokenProvider>();
			services.AddScoped<ISignalService, SignalService>();
			services.AddScoped<IUserApiService, UserApiService>();
			services.AddScoped<IStructureApiService, StructureApiService>();
		}

		public void Configure( IComponentsApplicationBuilder app ) {
			app.AddComponent<App>( "app" );
		}
	}
}
