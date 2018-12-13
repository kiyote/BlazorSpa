using System;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorSpa.Repository.Cognito {
	public static class CognitoExtensions {
		public static IServiceCollection AddCognito( this IServiceCollection services, CognitoOptions options) {
			services.AddSingleton( options );

			var provider = CreateProvider( options );
			services.AddSingleton( provider );

			return services;
		}

		public static IAmazonCognitoIdentityProvider CreateProvider(CognitoOptions options) {
			var chain = new CredentialProfileStoreChain( options.CredentialsFile );
			AWSCredentials credentials;
			if( !chain.TryGetAWSCredentials( options.CredentialsProfile, out credentials ) ) {
				throw new InvalidOperationException();
			}

			var roleCredentials = new AssumeRoleAWSCredentials(
				credentials,
				options.Role,
				Guid.NewGuid().ToString( "N" ) );

			AmazonCognitoIdentityProviderConfig config = new AmazonCognitoIdentityProviderConfig();
			config.RegionEndpoint = RegionEndpoint.GetBySystemName( options.RegionEndpoint );
			config.ServiceURL = options.ServiceUrl;
			config.LogMetrics = true;
			config.DisableLogging = false;
			var client = new AmazonCognitoIdentityProviderClient( roleCredentials, config );

			return client;
		}
	}
}
