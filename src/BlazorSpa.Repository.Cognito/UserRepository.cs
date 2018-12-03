using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using BlazorSpa.Model.Api;

namespace BlazorSpa.Repository.Cognito {
	public class UserRepository : IUserRepository {
		private readonly IAmazonCognitoIdentityProvider _client;
		private readonly CognitoOptions _cognitoOptions;

		public UserRepository(
			IAmazonCognitoIdentityProvider client,
			CognitoOptions cognitoOptions
		) {
			_client = client;
			_cognitoOptions = cognitoOptions;
		}

		public async Task<AuthenticationResult> Authenticate( string username, string password ) {
			var parameters = new Dictionary<string, string>() {
					{ "USERNAME", username },
					{ "PASSWORD", password }
			};

			try {
				var response = await _client.AdminInitiateAuthAsync( new AdminInitiateAuthRequest() {
					UserPoolId = _cognitoOptions.UserPoolId,
					ClientId = _cognitoOptions.ClientId,
					AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
					AuthParameters = parameters
				} );

				if( response.ChallengeName?.Value == "NEW_PASSWORD_REQUIRED" ) {
					return new AuthenticationResult( AuthenticationStatus.MustChangePassword, session: response.Session );

				} else if( response.AuthenticationResult != default ) {
					var authResult = response.AuthenticationResult;

					return new AuthenticationResult(
						AuthenticationStatus.Success,
						new AuthorizationToken() {
							access_token = authResult.AccessToken,
							token_type = authResult.TokenType,
							expires_in = authResult.ExpiresIn,
							refresh_token = authResult.RefreshToken
						} );
				} else {
					return new AuthenticationResult( AuthenticationStatus.Unknown );
				}
			} catch (NotAuthorizedException) {
				return new AuthenticationResult( AuthenticationStatus.Failure );
			} catch (UserNotFoundException) {
				return new AuthenticationResult( AuthenticationStatus.Failure );
			}
		}

		public async Task<CreateUserStatus> CreateUser( string username, string email ) {
			var attributes = new List<AttributeType>() {
				new AttributeType() {
					Name = "email",
					Value = email
				}
			};

			try {
				var response = await _client.AdminCreateUserAsync( new AdminCreateUserRequest() {
					UserPoolId = _cognitoOptions.UserPoolId,
					Username = username,
					UserAttributes = attributes
				} );

				if ( response.HttpStatusCode == System.Net.HttpStatusCode.OK ) {
					return CreateUserStatus.Success;
				} else {
					return CreateUserStatus.Unknown;
				}
			} catch (UsernameExistsException) {
				return CreateUserStatus.AlreadyExists;
			}
		}

		public async Task<bool> DeleteUser( string username ) {

			var disableResponse = await _client.AdminDisableUserAsync( new AdminDisableUserRequest() {
				Username = username,
				UserPoolId = _cognitoOptions.UserPoolId
			} );

			if ( disableResponse.HttpStatusCode != System.Net.HttpStatusCode.OK ) {
				return false;
			}

			var deleteResponse = await _client.AdminDeleteUserAsync( new AdminDeleteUserRequest() {
				Username = username,
				UserPoolId = _cognitoOptions.UserPoolId
			} );

			return ( deleteResponse.HttpStatusCode != System.Net.HttpStatusCode.OK );
		}

		public async Task<bool> ForceChangePassword(string username, string newPassword, string session) {
			var responses = new Dictionary<string, string>() {
				{ "USERNAME", username },
				{ "NEW_PASSWORD", newPassword }
			};

			var response = await _client.AdminRespondToAuthChallengeAsync( new AdminRespondToAuthChallengeRequest() {
				ChallengeName = "NEW_PASSWORD_REQUIRED",
				ClientId = _cognitoOptions.ClientId,
				UserPoolId = _cognitoOptions.UserPoolId,
				ChallengeResponses = responses,
				Session = session
			} );

			return ( response.HttpStatusCode == System.Net.HttpStatusCode.OK );
		}

		public async Task<UserInformationResult> GetUserInformation(string username) {
			var response = await _client.AdminGetUserAsync( new AdminGetUserRequest() {
				Username = username,
				UserPoolId = _cognitoOptions.UserPoolId
			} );

			var sub = response.UserAttributes.First( ua => ua.Name == "sub" );
			var email = response.UserAttributes.First( ua => ua.Name == "email" );
			var name = response.UserAttributes.FirstOrDefault( ua => ua.Name == "name" );

			return new UserInformationResult() {
				Username = name?.Value ?? username,
				UserId = new Guid(sub.Value).ToString("N"),
				Email = email.Value
			};
		}
	}
}
