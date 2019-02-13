using System;
using Amazon.DynamoDBv2.DataModel;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa" )]
	internal class StructureRecord {
		public const string StructureItemType = "Structure-";
		public const string Active = "Active";

		[DynamoDBHashKey( "PK" )]
		private string PK {
			get {
				return GetKey( StructureId );
			}
			set {
				StructureId = GetIdFromKey( value );
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
		public string StructureId { get; set; }

		[DynamoDBProperty("StructureType")]
		public string StructureType { get; set; }

		[DynamoDBProperty( "Status" )]
		public string Status { get; set; }

		[DynamoDBProperty("DateCreated")]
		public DateTime DateCreated { get; set; }

		public static string GetKey(string structureId) {
			return $"{StructureItemType}{structureId}";
		}

		public static string GetIdFromKey(string key) {
			return key.Substring( StructureItemType.Length );
		}
	}
}
