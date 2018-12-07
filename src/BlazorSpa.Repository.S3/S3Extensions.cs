using System;
using System.Linq;
using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorSpa.Repository.S3 {
	public static class S3Extensions {
		public static IServiceCollection AddS3( this IServiceCollection services, S3Options options ) {
			services.AddSingleton( options );

			var provider = CreateProvider( options );
			services.AddSingleton( provider );

			return services;
		}

		public static IAmazonS3 CreateProvider( S3Options options ) {
			var chain = new CredentialProfileStoreChain( options.CredentialsFile );
			AWSCredentials credentials;
			if( !chain.TryGetAWSCredentials( options.CredentialsProfile, out credentials ) ) {
				var profiles = chain.ListProfiles();
				throw new InvalidOperationException( profiles.Select( p => p.Name ).Aggregate( ( c, n ) => c + ", " + n ) );
			}

			AmazonS3Config config = new AmazonS3Config();
			config.RegionEndpoint = RegionEndpoint.GetBySystemName( options.RegionEndpoint );
			config.ServiceURL = options.ServiceUrl;
			config.LogMetrics = true;
			config.DisableLogging = false;
			return new AmazonS3Client( credentials, config );
		}
	}
}
