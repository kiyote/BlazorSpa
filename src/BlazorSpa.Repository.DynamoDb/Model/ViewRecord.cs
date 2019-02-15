using System;
using Amazon.DynamoDBv2.DataModel;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa" )]
	internal sealed class ViewRecord {

		public const string FixedViewId = ViewItemType + "00000000000000000000000000000000";
		public const string ViewItemType = "View-";

		[DynamoDBHashKey( "PK" )]
		private string PK {
			get {
				return FixedViewId;
			}
			set {
				// Do Nothing
			}
		}

		[DynamoDBRangeKey( "SK" )]
		private string SK {
			get {
				return GetKey( ViewId );
			}
			set {
				ViewId = GetIdFromKey( value );
			}
		}

		[DynamoDBIgnore]
		public string ViewId { get; set; }

		[DynamoDBProperty( "StructureType" )]
		public string ViewType { get; set; }

		[DynamoDBProperty( "DateCreated" )]
		public DateTime DateCreated { get; set; }

		public static string GetKey( string viewId ) {
			return $"{ViewItemType}{viewId}";
		}

		public static string GetIdFromKey( string key ) {
			return key.Substring( ViewItemType.Length );
		}
	}
}
