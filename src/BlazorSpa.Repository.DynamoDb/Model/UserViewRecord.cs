using System;
using Amazon.DynamoDBv2.DataModel;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa" )]
	internal sealed class UserViewRecord {

		[DynamoDBHashKey( "PK" )]
		private string PK {
			get {
				return UserRecord.GetKey( UserId );
			}
			set {
				UserId = UserRecord.GetIdFromKey( value );
			}
		}

		[DynamoDBRangeKey( "SK" )]
		private string SK {
			get {
				return ViewRecord.GetKey( ViewId );
			}
			set {
				ViewId = ViewRecord.GetIdFromKey( value );
			}
		}

		[DynamoDBIgnore]
		public string UserId { get; set; }

		[DynamoDBIgnore]
		public string ViewId { get; set; }

		[DynamoDBProperty( "DateCreated" )]
		public DateTime DateCreated { get; set; }
	}
}
