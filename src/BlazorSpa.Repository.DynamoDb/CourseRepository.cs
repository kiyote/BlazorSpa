using System;
using System.Collections.Generic;
using System.Text;
using BlazorSpa.Repository.Model;

namespace BlazorSpa.Repository.DynamoDb {
	public sealed class CourseRepository : ICourseRepository {
		IEnumerable<Course> ICourseRepository.GetCoursesByUser( Id<User> userId ) {
			throw new NotImplementedException();
		}
	}
}
