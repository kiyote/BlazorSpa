using System;
using System.Collections.Generic;
using BlazorSpa.Repository.Model;
using BlazorSpa.Shared;

namespace BlazorSpa.Repository.DynamoDb {
	public sealed class CourseRepository : ICourseRepository {
		IEnumerable<Course> ICourseRepository.GetCoursesByUser( Id<User> userId ) {
			throw new NotImplementedException();
		}
	}
}
