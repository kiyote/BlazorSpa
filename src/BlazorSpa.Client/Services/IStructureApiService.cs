using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorSpa.Client.Model;
using BlazorSpa.Shared;

namespace BlazorSpa.Client.Services {
	public interface IStructureApiService {

		Task<IEnumerable<View>> GetAllViews();

		Task<IEnumerable<View>> GetUserViews();

		Task<View> CreateView( string viewType, string viewName );

		Task<Structure> CreateStructureInView( Id<View> viewId, string structureType, string name );

		Task AddStructureToView( Id<View> viewId, Id<Structure> structureId );

		Task<IEnumerable<Structure>> GetViewStructures( Id<View> viewId );

		Task<IEnumerable<Structure>> GetChildStructures( Id<View> viewId, Id<Structure> structureId );

		Task<Structure> CreateChildStructure( Id<View> viewId, Id<Structure> parentStructureId, string structureType, string name );

		Task<Structure> GetParentStructure( Id<View> viewId, Id<Structure> structureId );
	}
}
