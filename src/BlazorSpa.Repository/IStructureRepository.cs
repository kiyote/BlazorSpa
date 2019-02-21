using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorSpa.Repository.Model;
using BlazorSpa.Shared;

namespace BlazorSpa.Repository {
	public interface IStructureRepository {
		Task<Structure> AddStructure( Id<Structure> structureId, string structureType, DateTime dateCreated );

		Task<IEnumerable<Structure>> GetStructures( IEnumerable<Id<Structure>> structureIds );

		Task AddChildStructure( Id<View> viewId, Id<Structure> parentStructureId, Id<Structure> childStructureId, DateTime dateCreated );

		Task<IEnumerable<Id<Structure>>> GetChildStructureIds( Id<View> viewId, Id<Structure> structureId );

		Task<View> AddView( Id<View> viewId, string viewType, string name, DateTime dateCreated );

		Task<IEnumerable<View>> GetViews( IEnumerable<Id<View>> viewIds );

		Task<IEnumerable<Id<View>>> GetViewIds();

		Task<IEnumerable<Id<Structure>>> GetViewStructureIds( Id<View> viewId );

		Task<StructureOperationStatus> AddViewStructure( Id<View> viewId, Id<Structure> structureId, DateTime dateCreated );
	}
}