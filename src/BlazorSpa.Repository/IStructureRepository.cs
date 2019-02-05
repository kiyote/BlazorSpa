using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlazorSpa.Repository.Model;

namespace BlazorSpa.Repository {
	public interface IStructureRepository {
		Task<Structure> Add( Id<Structure> structureId, string structureType );
	}
}
