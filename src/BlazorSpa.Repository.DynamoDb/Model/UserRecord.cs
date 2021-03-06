﻿/*
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
	internal sealed class UserRecord {

		private const string UserItemType = "User-";
		public readonly static string Active = "Active";

		[DynamoDBHashKey( "PK" )]
		private string PK {
			get {
				return GetKey( UserId );
			}
			set {
				UserId = value.Substring( UserItemType.Length );
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
		public string UserId { get; set; }

		[DynamoDBProperty("Username")]
		public string Name { get; set; }

		[DynamoDBProperty("HasAvatar")]
		public bool HasAvatar { get; set; }

		[DynamoDBProperty("LastLogin")]
		public DateTime LastLogin { get; set; }

		[DynamoDBProperty("PreviousLogin")]
		public DateTime? PreviousLogin { get; set; }

		[DynamoDBProperty( "Status" )]
		public string Status { get; set; }

		public static string GetKey(string userId) {
			return $"{UserItemType}{userId}";
		}

		public static string GetIdFromKey( string key ) {
			return key.Substring( UserItemType.Length );
		}
	}
}
