using System;
using Amazon.DynamoDBv2.DataModel;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa" )]
	internal sealed class ChildStructureRecord {

		[DynamoDBHashKey( "PK" )]
		private string PK {
			get {
				return StructureRecord.GetKey( ParentStructureId );
			}
			set {
				ParentStructureId = StructureRecord.GetIdFromKey( value );
			}
		}

		[DynamoDBRangeKey( "SK" )]
		private string SK {
			get {
				return GetKey( ViewId, ChildStructureId );
			}
			set {
				var values = value.Split( '|' );
				ViewId = ViewRecord.GetIdFromKey( values[ 0 ] );
				ChildStructureId = StructureRecord.GetIdFromKey( values[1] );
			}
		}

		[DynamoDBIgnore]
		public string ViewId { get; set; }

		[DynamoDBIgnore]
		public string ParentStructureId { get; set; }

		[DynamoDBIgnore]
		public string ChildStructureId { get; set; }

		[DynamoDBProperty( "DateCreated" )]
		public DateTime DateCreated { get; set; }

		public static string GetKey( string viewId, string structureId ) {
			return $"{ViewRecord.GetKey( viewId )}|{StructureRecord.GetKey(structureId)}";
		}
	}
}
