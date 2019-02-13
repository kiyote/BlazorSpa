using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.DataModel;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa" )]
	internal sealed class ViewRecord {

		public const string ViewItemType = "View-";

		[DynamoDBHashKey( "PK" )]
		private string PK {
			get {
				return GetKey( ViewId );
			}
			set {
				ViewId = GetIdFromKey( value );
			}
		}

		[DynamoDBRangeKey( "SK" )]
		public string SK {
			get {
				return PK;
			}
			set {
				// Do nothing
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
