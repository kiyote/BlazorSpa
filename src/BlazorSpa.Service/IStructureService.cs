using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorSpa.Repository.Model;

namespace BlazorSpa.Service {
	public interface IStructureService {
		Task<IEnumerable<View>> GetUserViews( Id<User> userId );

		Task<IEnumerable<View>> GetAllViews();

		Task<IEnumerable<Structure>> GetViewRootStrutures( Id<View> viewId );
	}
}
