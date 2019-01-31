using System;
using Amazon.DynamoDBv2.DataModel;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa" )]
	public sealed class UserRecord {

		public readonly static string UserItemType = "User";
		public readonly static string Active = "Active";

		public UserRecord() {
			ItemType = UserItemType;
		}

		[DynamoDBHashKey("PK")]
		public string UserId { get; set; }

		[DynamoDBRangeKey( "SK" )]
		public string ItemType { get; set; }

		[DynamoDBProperty("Username")]
		public string Name { get; set; }

		[DynamoDBProperty("HasAvatar")]
		public bool HasAvatar { get; set; }

		[DynamoDBProperty("LastLogin")]
		public DateTime LastLogin { get; set; }

		[DynamoDBProperty("PreviousLogin")]
		public DateTime? PreviousLogin { get; set; }

		[DynamoDBProperty( "Status" )]
		public string Status { get; set; }
	}
}
