using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using BlazorSpa.Repository.Model;

namespace BlazorSpa.Repository.Cognito {
	public sealed class AuthenticationRepository : IAuthenticationRepository {

		private readonly IAmazonCognitoIdentityProvider _client;
		private readonly CognitoOptions _cognitoOptions;

		public AuthenticationRepository(
			IAmazonCognitoIdentityProvider client,
			CognitoOptions cognitoOptions
		) {
			_client = client;
			_cognitoOptions = cognitoOptions;
		}

		async Task<AuthenticationUserInformation> IAuthenticationRepository.GetUserInformation(string username) {
			var response = await _client.AdminGetUserAsync( new AdminGetUserRequest() {				
				Username = username,
				UserPoolId = _cognitoOptions.UserPoolId
			} );

			var sub = response.UserAttributes.First( ua => ua.Name == "sub" );
			var email = response.UserAttributes.First( ua => ua.Name == "email" );
			var name = response.UserAttributes.FirstOrDefault( ua => ua.Name == "name" );

			return new AuthenticationUserInformation(
				new Guid( sub.Value ).ToString( "N" ),
				name?.Value ?? username,
				email.Value
			);
		}
	}
}
