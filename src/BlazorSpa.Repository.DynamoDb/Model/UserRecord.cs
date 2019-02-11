using System;
using Amazon.DynamoDBv2.DataModel;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa" )]
	public sealed class UserRecord {

		public readonly static string UserItemType = "User";
		public readonly static string Active = "Active";

		[DynamoDBHashKey( "PK" )]
		public string PK {
			get {
				return $"{UserItemType}-{UserId}";
			}
			set {
				UserId = value.Split( '-' )[ 1 ];
			}
		}

		[DynamoDBRangeKey( "SK" )]
		public string SK {
			get {
				return PK;
			}
			set {
				PK = value;
			}
		}

		[DynamoDBIgnore]
		public string UserId { get; set; }

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

		public static string GetKey(string userId) {
			return $"{UserItemType}-{userId}";
		}
	}
}
