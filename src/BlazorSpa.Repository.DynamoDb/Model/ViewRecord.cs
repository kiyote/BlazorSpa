/*
 * Copyright 2018-2019 Todd Lang
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
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

		[DynamoDBProperty( "Name" )]
		public string Name { get; set; }

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
