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
