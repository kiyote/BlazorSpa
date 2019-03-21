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

		public static string ChildStructureType(string viewId) {
			return $"{ViewRecord.GetKey( viewId )}|{StructureRecord.StructureItemType}";
		}
	}
}
