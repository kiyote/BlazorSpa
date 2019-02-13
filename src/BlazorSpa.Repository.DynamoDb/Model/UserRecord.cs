using System;
using Amazon.DynamoDBv2.DataModel;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa" )]
	internal sealed class UserRecord {

		private const string UserItemType = "User-";
		public readonly static string Active = "Active";

		[DynamoDBHashKey( "PK" )]
		private string PK {
			get {
				return GetKey( UserId );
			}
			set {
				UserId = value.Substring( UserItemType.Length );
			}
		}

		[DynamoDBRangeKey( "SK" )]
		private string SK {
			get {
				return PK;
			}
			set {
				// Do nothing
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
			return $"{UserItemType}{userId}";
		}

		public static string GetIdFromKey( string key ) {
			return key.Substring( UserItemType.Length );
		}
	}
}
