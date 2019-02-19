using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorSpa.Repository.Model;

namespace BlazorSpa.Repository {
	public interface IStructureRepository {
		Task<Structure> AddStructure( Id<Structure> structureId, string structureType, DateTimeOffset dateCreated );

		Task<IEnumerable<Structure>> GetStructures( IEnumerable<Id<Structure>> structureIds );

		Task AddChildStructure( Id<View> viewId, Id<Structure> parentStructureId, Id<Structure> childStructureId, DateTimeOffset dateCreated );

		Task<IEnumerable<Id<Structure>>> GetChildStructureIds( Id<View> viewId, Id<Structure> structureId );

		Task<View> AddView( Id<View> viewId, string viewType, DateTimeOffset dateCreated );

		Task<IEnumerable<View>> GetViews( IEnumerable<Id<View>> viewIds );

		Task<IEnumerable<Id<View>>> GetViewIds();

		Task<IEnumerable<Id<Structure>>> GetViewStructureIds( Id<View> viewId );

		Task AddViewStructure( Id<View> viewId, Id<Structure> structureId, DateTimeOffset dateCreated );
	}
}