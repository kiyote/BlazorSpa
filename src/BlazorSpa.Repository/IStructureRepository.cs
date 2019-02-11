using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlazorSpa.Repository.Model;

namespace BlazorSpa.Repository {
	public interface IStructureRepository {
		Task<Structure> Add( Id<Structure> structureId, string structureType );

		Task Organize( Id<Structure> parentStructureId, Id<Structure> childStructureId );

		Task<IEnumerable<Id<Structure>>> GetHomeStructureIds( Id<User> userId );

		Task<IEnumerable<Structure>> GetStructures( IEnumerable<Id<Structure>> structureIds );
	}
}