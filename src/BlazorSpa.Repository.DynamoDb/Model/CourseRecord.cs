using System;
using Amazon.DynamoDBv2.DataModel;

namespace BlazorSpa.Repository.DynamoDb.Model {
	[DynamoDBTable( "BlazorSpa" )]
	public class CourseRecord {

		public readonly static string CourseItemType = "Course";
		public readonly static string Active = "Active";

		public CourseRecord() {
			ItemType = CourseItemType;
		}

		[DynamoDBHashKey( "PK" )]
		public string CourseId { get; set; }

		[DynamoDBRangeKey( "SK" )]
		public string ItemType { get; set; }

		[DynamoDBProperty( "Status" )]
		public string Status { get; set; }
	}
}
