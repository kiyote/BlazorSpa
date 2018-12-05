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
			loggerFactory.AddAWSProvider( new AWSLoggerConfig() {
				Region = options.Region,
				LogGroup = options.LogGroup,
				Credentials = credentials,
				LogStreamNameSuffix = options.StreamName
			}, LogLevel.Debug );
		}
	}
}
