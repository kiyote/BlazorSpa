using System;
using Amazon.DynamoDBv2.DataModel;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa" )]
	public class AuthenticationRecord {

		public AuthenticationRecord() {
		}

		[DynamoDBHashKey( "PK" )]
		public string Username { get; set; }

		[DynamoDBRangeKey( "SK" )]
		public string Status { get; set; }

		[DynamoDBProperty("UserId")]
		public string UserId { get; set; }
	}
}
