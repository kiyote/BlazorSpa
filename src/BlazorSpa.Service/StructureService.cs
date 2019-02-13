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

		public Task<IEnumerable<Structure>> GetHomeStructures( Id<User> userId ) {
			return Task.FromResult(default(IEnumerable<Structure>));
		}
	}
}
