using System;
using Amazon.DynamoDBv2.DataModel;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa" )]
	internal sealed class UserRecord {

		public readonly static string Active = "Active";

		[DynamoDBHashKey("PK")]
		public string UserId { get; set; }

		[DynamoDBRangeKey("SK")]
		public string SK {
			get {
				return $"{Status}/{Name}";
			}
			set {
				var values = value.Split( '/' );
				Status = values[ 0 ];
				Name = values[ 1 ];
			}
		}

		[DynamoDBProperty("AuthenticationId")]
		public string AuthenticationId { get; set; }

		[DynamoDBIgnore]
		public string Name { get; set; }

		[DynamoDBIgnore]
		public string Status { get; set; }
	}
}
