using System;
using Amazon.DynamoDBv2.DataModel;
using BlazorSpa.Model;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa_Users" )]
	internal sealed class DynamoUser {

		public DynamoUser() {
		}

		public DynamoUser(User user)
			: this(user.Id, user.AuthenticationId) {
		}

		public DynamoUser(
			Id<User> userId, 
			string authenticationId
		) {
			UserId = userId.Value;
			AuthenticationId = authenticationId;
		}

		[DynamoDBHashKey]
		public string UserId { get; set; }

		[DynamoDBGlobalSecondaryIndexHashKey]
		public string AuthenticationId { get; set; }

		public User ToUser() {
			return new User( new Id<User>( UserId ), AuthenticationId );
		}
	}
}
