using System;
using Amazon.DynamoDBv2.DataModel;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa" )]
	public class AuthenticationRecord {

		public static readonly string AuthenticationItemType = "Authentication";
		public static readonly string StatusActive = "Active";

		public AuthenticationRecord() {
			ItemType = AuthenticationItemType;
		}

		[DynamoDBHashKey( "PK" )]
		public string Identifier { get; set; }

		[DynamoDBRangeKey( "SK" )]
		public string ItemType { get; set; }

		[DynamoDBProperty("UserId")]
		public string UserId { get; set; }

		[DynamoDBProperty("DateCreated")]
		public DateTime DateCreated { get; set; }

		[DynamoDBProperty("Status")]
		public string Status { get; set; }
	}
}
