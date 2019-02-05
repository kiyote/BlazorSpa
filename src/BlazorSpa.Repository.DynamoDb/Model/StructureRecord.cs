using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.DataModel;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa" )]
	public class StructureRecord {
		public readonly static string StructureItemType = "Structure";
		public readonly static string Active = "Active";

		public StructureRecord() {
			ItemType = StructureItemType;
		}

		[DynamoDBHashKey( "PK" )]
		public string StructureId { get; set; }

		[DynamoDBRangeKey( "SK" )]
		public string ItemType { get; set; }

		[DynamoDBProperty("StructureType")]
		public string StructureType { get; set; }

		[DynamoDBProperty( "Status" )]
		public string Status { get; set; }
	}
}
