using System;
using Amazon.DynamoDBv2.DataModel;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa" )]
	public sealed class AuthenticationRecord {

		public static readonly string AuthenticationItemType = "Authentication";
		public static readonly string StatusActive = "Active";

		[DynamoDBHashKey( "PK" )]
		public string PK {
			get {
				return $"{AuthenticationItemType}-{Username}";
			}
			set {
				Username = value.Split( '-' )[ 1 ];
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
		public string Username { get; set; }

		[DynamoDBProperty("UserId")]
		public string UserId { get; set; }

		[DynamoDBProperty("DateCreated")]
		public DateTime DateCreated { get; set; }

		[DynamoDBProperty("Status")]
		public string Status { get; set; }
	}
}
