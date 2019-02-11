using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.DataModel;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa" )]
	public sealed class HomeStructureRecord {

		[DynamoDBHashKey("PK")]
		public string UserId { get; set; }

		[DynamoDBRangeKey("SK")]
		public string StructureId { get; set; }

		[DynamoDBProperty( "DateCreated" )]
		public DateTime DateCreated { get; set; }
	}
}
