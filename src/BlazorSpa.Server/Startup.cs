using System;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BlazorSpa.Logging.CloudWatch;
using BlazorSpa.Repository;
using BlazorSpa.Repository.Cognito;
using BlazorSpa.Repository.DynamoDb;
using BlazorSpa.Server.Hubs;
using BlazorSpa.Server.Managers;
using BlazorSpa.Server.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Blazor.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

namespace BlazorSpa.Server {
	public class Startup {

		public Startup( IConfiguration configuration ) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices( IServiceCollection services ) {

			services.AddLogging( builder => builder
				.SetMinimumLevel( LogLevel.Debug )
			);

			services
				.AddConnections()
				.AddSignalR( o => o.KeepAliveInterval = TimeSpan.FromSeconds( 5 ) )
				.AddJsonProtocol();

			services.AddAuthorization( options => {
				options.AddPolicy( JwtBearerDefaults.AuthenticationScheme, policy => {
					policy.AddAuthenticationSchemes( JwtBearerDefaults.AuthenticationScheme );
					policy.RequireClaim( ClaimTypes.NameIdentifier );
				} );
			} );

			services
				.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
				.AddJwtBearer( JwtBearerDefaults.AuthenticationScheme, SetJwtBearerOptions );

			services.AddCors( options => options.AddPolicy( "CorsPolicy",
				builder => {
					builder.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowAnyOrigin()
						.AllowCredentials();
				} ) );

			services.AddResponseCompression( options => {
				options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat( new[]
				{
					MediaTypeNames.Application.Octet,
					WasmMediaTypeNames.Application.Wasm,
				} );
			} );

			services.AddMvc()
				.SetCompatibilityVersion( CompatibilityVersion.Version_2_1 )
				.AddJsonOptions( options => {
					options.SerializerSettings.ContractResolver = new DefaultContractResolver();
				} );

			services.AddDynamoDb( Configuration.GetSection( "DynamoDb" ).Get<DynamoDbOptions>() );
			services.AddCognito( Configuration.GetSection( "Cognito" ).Get<CognitoOptions>() );

			services.AddSingleton<IUserRepository, UserRepository>();
			services.AddSingleton<AuthenticationManager>();
			services.AddSingleton<UserManager>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(
			IApplicationBuilder app,
			IHostingEnvironment env,
			ILoggerFactory loggerFactory
		) {
			if( env.IsDevelopment() ) {
				app.UseDeveloperExceptionPage();
			} else {
				app.UseCloudWatchLogging(
					loggerFactory,
					Configuration.GetSection( "CloudWatchLogs" ).Get<CloudWatchOptions>() );
			}

			app.UseResponseCompression();
			app.UseAuthentication();
			app.UseIdentificationMiddleware();

			app.UseCors( "CorsPolicy" );
			app.UseSignalR( routes => {
				routes.MapHub<SignalHub>( SignalHub.Url );
			} );

			app.UseMvc( routes => {
				routes.MapRoute( name: "default", template: "{controller}/{action}/{id?}" );
			} );

			app.UseBlazor<Client.Startup>();
		}

		private void SetJwtBearerOptions( JwtBearerOptions options ) {
			var tokenValidationOptions = Configuration.GetSection( "TokenValidation" ).Get<TokenValidationOptions>();
			var rsa = new RSACryptoServiceProvider();
			rsa.ImportParameters(
				new RSAParameters() {
					Modulus = Base64UrlEncoder.DecodeBytes( tokenValidationOptions.Modulus ),
					Exponent = Base64UrlEncoder.DecodeBytes( tokenValidationOptions.Expo )
				} );
			var key = new RsaSecurityKey( rsa );

			options.RequireHttpsMetadata = false;
			options.TokenValidationParameters = new TokenValidationParameters {
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = key,
				ValidIssuer = tokenValidationOptions.Issuer,
				ValidateIssuer = true,
				ValidateLifetime = true,
				ValidateAudience = false,
				ClockSkew = TimeSpan.FromMinutes( 0 )
			};

			options.Events = new JwtBearerEvents {
				OnMessageReceived = context => {
					var accessToken = context.Request.Query[ "access_token" ];

					// If there is an access token supplied in the url, then
					// we check to see if we're actually trying to service
					// a SignalR request, and if so, we tuck the token in to
					// the context so the request is property authenticated.
					if( !string.IsNullOrWhiteSpace( accessToken )
						&& ( context.HttpContext.WebSockets.IsWebSocketRequest
							|| context.HttpContext.Request.Path.StartsWithSegments( SignalHub.Url )
							|| context.Request.Headers[ "Accept" ] == "text/event-stream" ) ) {
						context.Token = context.Request.Query[ "access_token" ];
					}
					return Task.CompletedTask;
				}
			};
		}
	}
}
