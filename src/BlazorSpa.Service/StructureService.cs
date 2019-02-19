using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorSpa.Repository;
using BlazorSpa.Repository.Model;

namespace BlazorSpa.Service {
	internal sealed class StructureService : IStructureService {

		private readonly IStructureRepository _structureRepository;
		private readonly IUserRepository _userRepository;

		public StructureService(
			IStructureRepository structureRepository,
			IUserRepository userRepository
		) {
			_structureRepository = structureRepository;
			_userRepository = userRepository;
		}

		async Task<IEnumerable<Structure>> IStructureService.GetViewRootStrutures( Id<View> viewId ) {
			var structureIds = await _structureRepository.GetViewStructureIds( viewId );
			var structures = await _structureRepository.GetStructures( structureIds );

			return structures;
		}

		async Task<IEnumerable<View>> IStructureService.GetUserViews( Id<User> userId ) {
			var viewIds = await _userRepository.GetViewIds( userId );
			var views = await _structureRepository.GetViews( viewIds );

			return views;
		}

		async Task<IEnumerable<View>> IStructureService.GetAllViews() {
			var viewIds = await _structureRepository.GetViewIds();
			var views = await _structureRepository.GetViews( viewIds );

			return views;
		}
	}
}
