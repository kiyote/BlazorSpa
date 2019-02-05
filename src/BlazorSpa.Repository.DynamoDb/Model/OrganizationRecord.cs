using System;
using Amazon.DynamoDBv2.DataModel;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa" )]
	public class OrganizationRecord {
		public readonly static string Active = "Active";

		public OrganizationRecord() {
		}

		[DynamoDBHashKey( "PK" )]
		public string StructureId { get; set; }

		[DynamoDBRangeKey( "SK" )]
		public string ChildStructureId { get; set; }

		[DynamoDBProperty( "Status" )]
		public string Status { get; set; }
	}
}
