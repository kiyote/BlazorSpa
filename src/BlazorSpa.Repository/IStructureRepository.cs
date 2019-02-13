using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorSpa.Repository.Model;

namespace BlazorSpa.Repository {
	public interface IStructureRepository {
		Task<Structure> Add( Id<Structure> structureId, string structureType, DateTimeOffset dateCreated );

		Task<IEnumerable<Structure>> GetStructures( IEnumerable<Id<Structure>> structureIds );

		Task AddChild( Id<View> viewId, Id<Structure> parentStructureId, Id<Structure> childStructureId, DateTimeOffset dateCreated );

		Task<IEnumerable<Id<View>>> GetUserViewIds( Id<User> userId );

		Task<IEnumerable<View>> GetViews( IEnumerable<Id<View>> viewIds );
	}
}