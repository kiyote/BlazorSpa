using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorSpa.Repository;
using BlazorSpa.Repository.Model;

namespace BlazorSpa.Service {
	internal sealed class StructureService : IStructureService {

		private readonly IStructureRepository _structureRepository;

		public StructureService(
			IStructureRepository structureRepository
		) {
			_structureRepository = structureRepository;
		}

		public async Task<IEnumerable<Structure>> GetHomeStructures( Id<User> userId ) {
			var ids = await _structureRepository.GetHomeStructureIds( userId );
			return await _structureRepository.GetStructures( ids );
		}
	}
}
