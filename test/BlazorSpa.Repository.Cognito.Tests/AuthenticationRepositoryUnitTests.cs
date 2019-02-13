using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Moq;
using NUnit.Framework;

namespace BlazorSpa.Repository.Cognito.Tests {
	public class AuthenticationRepositoryUnitTests {

		private string TestUserPoolId = "testuserpoolid";
		private IAuthenticationRepository _authenticationRepository;
		private Mock<IAmazonCognitoIdentityProvider> _cognito;

		[SetUp]
		public void Setup() {
			_cognito = new Mock<IAmazonCognitoIdentityProvider>();
			var cognitoOptions = new CognitoOptions() {
				UserPoolId = TestUserPoolId
			};
			_authenticationRepository = new AuthenticationRepository( _cognito.Object, cognitoOptions );
		}

		[Test]
		public async Task GetUserInformation_WithNameAttribute_NameReturned() {
			var expectedUsername = "test";
			string actualUsername = default;
			string actualUserPoolId = default;
			var expectedName = "name";
			var expectedEmail = "email";
			var expectedSub = Guid.NewGuid();

			var response = new AdminGetUserResponse() {
				UserAttributes = new List<AttributeType>() {
					new AttributeType() {
						Name = "sub",
						Value = expectedSub.ToString()
					},
					new AttributeType() {
						Name = "email",
						Value = expectedEmail
					},
					new AttributeType() {
						Name = "name",
						Value = expectedName
					}
				}
			};
			_cognito
				.Setup( c => c.AdminGetUserAsync( It.IsAny<AdminGetUserRequest>(), default ))
				.Callback<AdminGetUserRequest, CancellationToken>( (req, tok) => { actualUsername = req.Username; actualUserPoolId = req.UserPoolId; } )
				.Returns( Task.FromResult( response ) );

			var information = await _authenticationRepository.GetUserInformation( expectedUsername );

			Assert.AreEqual( expectedUsername, actualUsername );
			Assert.AreEqual( TestUserPoolId, actualUserPoolId );
			Assert.AreEqual( expectedName, information.Username );
			Assert.AreEqual( expectedEmail, information.Email );
			Assert.AreEqual( expectedSub.ToString("N"), information.AuthenticationId );
		}
	}
}