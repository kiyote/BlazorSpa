using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorSpa.Repository;
using BlazorSpa.Repository.Model;
using BlazorSpa.Shared;

namespace BlazorSpa.Service {
	public interface IStructureService {
		Task<IEnumerable<View>> GetUserViews( Id<User> userId );

		Task<IEnumerable<View>> GetAllViews();

		Task<IEnumerable<Structure>> GetViewRootStrutures( Id<View> viewId );

		Task<Structure> CreateStructure( Id<Structure> structureId, string structureType, string name, DateTime createdOn );

		Task<StructureOperationStatus> AddStructureToView( Id<Structure> structureId, Id<View> viewId, DateTime createdOn );

		Task<View> CreateViewWithUser( Id<User> userId, Id<View> viewId, string viewType, string viewName, DateTime createdOn );
	}
}
