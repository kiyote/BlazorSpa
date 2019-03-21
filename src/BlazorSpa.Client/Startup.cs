/*
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
using BlazorSpa.Client.Services;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlazorSpa.Client {
	public sealed class Startup {
		public void ConfigureServices( IServiceCollection services ) {
			services.AddLogging( builder => builder
				 .SetMinimumLevel( LogLevel.Trace )
			);

			services.AddSingleton<IJsonConverter, JsonConverter>();
			services.AddSingleton<IConfig, Config>();

			services.AddScoped<IAppState, AppState>();
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
