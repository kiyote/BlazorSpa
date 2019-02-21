using System.Collections.Generic;
using BlazorSpa.Repository.Model;
using BlazorSpa.Shared;

namespace BlazorSpa.Repository {
	public interface ICourseRepository {
		IEnumerable<Course> GetCoursesByUser( Id<User> userId );
	}
}
