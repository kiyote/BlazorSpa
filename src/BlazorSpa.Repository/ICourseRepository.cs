using System;
using System.Collections.Generic;
using BlazorSpa.Repository.Model;

namespace BlazorSpa.Repository {
	public interface ICourseRepository {
		IEnumerable<Course> GetCoursesByUser( Id<User> userId );
	}
}
