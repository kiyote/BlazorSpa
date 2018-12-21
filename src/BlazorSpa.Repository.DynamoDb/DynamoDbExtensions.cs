using System;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorSpa.Repository.DynamoDb {
	public static class DynamoDbExtensions {

		public static IServiceCollection AddDynamoDb( this IServiceCollection services, DynamoDbOptions options ) {
			services.AddSingleton( options );

			var provider = CreateProvider( options );
			services.AddSingleton( provider );

			return services;
		}

		public static IDynamoDBContext CreateProvider( DynamoDbOptions options ) {
			var chain = new CredentialProfileStoreChain( options.CredentialsFile );
			if( !chain.TryGetAWSCredentials( options.CredentialsProfile, out AWSCredentials credentials ) ) {
				throw new InvalidOperationException();
			}
			var roleCredentials = new AssumeRoleAWSCredentials( 
				credentials, 
				options.Role, 
				Guid.NewGuid().ToString("N") );

			AmazonDynamoDBConfig config = new AmazonDynamoDBConfig();
			config.RegionEndpoint = RegionEndpoint.GetBySystemName( options.RegionEndpoint );
			config.ServiceURL = options.ServiceUrl;
			config.LogMetrics = true;
			config.DisableLogging = false;
			var client = new AmazonDynamoDBClient( roleCredentials, config );
			return new DynamoDBContext( client );
		}
	}
}
