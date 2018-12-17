using System;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using AWS.Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace BlazorSpa.Logging.CloudWatch {
	public static class CloudWatchExtensions {
		public static IApplicationBuilder UseCloudWatchLogging( 
			this IApplicationBuilder services, 
			ILoggerFactory loggerFactory, 
			CloudWatchOptions options 
		) {
			RegisterCloudWatchLogging( loggerFactory, options );

			return services;
		}

		public static void RegisterCloudWatchLogging( ILoggerFactory loggerFactory, CloudWatchOptions options ) {
			var chain = new CredentialProfileStoreChain( options.CredentialsFile );
			AWSCredentials credentials;
			if( !chain.TryGetAWSCredentials( options.CredentialsProfile, out credentials ) ) {
				throw new InvalidOperationException();
			}

			var roleCredentials = new AssumeRoleAWSCredentials(
				credentials,
				options.Role,
				Guid.NewGuid().ToString( "N" ) );

			if (!Enum.TryParse(options.LogLevel, out LogLevel logLevel)) {
				logLevel = LogLevel.Debug;
			}
			
			loggerFactory.AddAWSProvider( new AWSLoggerConfig() {
				Region = options.Region,
				LogGroup = options.LogGroup,
				Credentials = roleCredentials,
				LogStreamNameSuffix = options.StreamName
			}, logLevel );
		}
	}
}
