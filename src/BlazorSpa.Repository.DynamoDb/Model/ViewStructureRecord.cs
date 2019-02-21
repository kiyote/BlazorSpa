﻿using System;
using Amazon.DynamoDBv2.DataModel;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa" )]
	internal sealed class ViewStructureRecord {

		[DynamoDBHashKey( "PK" )]
		private string PK {
			get {
				return ViewRecord.GetKey( ViewId );
			}
			set {
				ViewId = ViewRecord.GetIdFromKey( value );
			}
		}

		[DynamoDBRangeKey( "SK" )]
		private string SK {
			get {
				return StructureRecord.GetKey( StructureId );
			}
			set {
				StructureId = StructureRecord.GetIdFromKey( value );
			}
		}

		[DynamoDBIgnore]
		public string ViewId { get; set; }

		[DynamoDBIgnore]
		public string StructureId { get; set; }

		[DynamoDBProperty( "DateCreated" )]
		public DateTime DateCreated { get; set; }
	}
}