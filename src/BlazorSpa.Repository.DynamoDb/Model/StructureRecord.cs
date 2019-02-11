using System;
using Amazon.DynamoDBv2.DataModel;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa" )]
	public class StructureRecord {
		public readonly static string StructureItemType = "Structure";
		public readonly static string Active = "Active";

		[DynamoDBHashKey( "PK" )]
		public string PK {
			get {
				return GetKey( StructureId );
			}
			set {
				StructureId = value.Split( '-' )[ 1 ];
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
		public string StructureId { get; set; }

		[DynamoDBProperty("StructureType")]
		public string StructureType { get; set; }

		[DynamoDBProperty( "Status" )]
		public string Status { get; set; }

		public static string GetKey(string structureId) {
			return $"{StructureItemType}-{structureId}";
		}
	}
}
