using System;
using Amazon.DynamoDBv2.DataModel;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa" )]
	public class UserRecord {

		public readonly static string Active = "Active";

		public UserRecord() {
		}

		[DynamoDBHashKey("PK")]
		public string UserId { get; set; }

		[DynamoDBRangeKey( "SK" )]
		public string Status { get; set; }

		[DynamoDBProperty("Username")]
		public string Name { get; set; }

		[DynamoDBProperty("HasAvatar")]
		public bool HasAvatar { get; set; }

		[DynamoDBProperty("LastLogin")]
		public DateTime LastLogin { get; set; }
	}
}
