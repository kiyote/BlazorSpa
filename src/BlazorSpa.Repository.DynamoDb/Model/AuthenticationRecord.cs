using System;
using Amazon.DynamoDBv2.DataModel;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa" )]
	internal sealed class AuthenticationRecord {

		private const string AuthenticationItemType = "Authentication-";
		public static readonly string StatusActive = "Active";

		[DynamoDBHashKey( "PK" )]
		private string PK {
			get {
				return GetKey( Username );
			}
			set {
				Username = value.Substring( AuthenticationItemType.Length );
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
		public string Username { get; set; }

		[DynamoDBProperty("UserId")]
		public string UserId { get; set; }

		[DynamoDBProperty("DateCreated")]
		public DateTime DateCreated { get; set; }

		[DynamoDBProperty("Status")]
		public string Status { get; set; }

		public static string GetKey(string username) {
			return $"{AuthenticationItemType}{username}";
		}
	}
}
