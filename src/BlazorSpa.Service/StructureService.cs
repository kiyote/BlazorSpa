using System;
using System.Collections.Generic;
using System.Linq;
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

		async Task<Structure> IStructureService.CreateStructure( Id<Structure> structureId, string structureType, DateTimeOffset dateCreated ) {
			var structureIds = new List<Id<Structure>>() {
				structureId
			};
			var result = await _structureRepository.GetStructures( structureIds );
			if (result.Any()) {
				return result.First();
			}

			return await _structureRepository.AddStructure( structureId, structureType, dateCreated );
		}

		async Task<StructureOperationStatus> IStructureService.AddStructureToView(Id<Structure> structureId, Id<View> viewId, DateTimeOffset dateCreated ) {
			var structures = await _structureRepository.GetViewStructureIds( viewId );
			if (structures.Contains(structureId)) {
				return StructureOperationStatus.AlreadyExists;
			}

			return await _structureRepository.AddViewStructure( viewId, structureId, dateCreated );
		}

		async Task<View> IStructureService.CreateViewWithUser(Id<User> userId, Id<View> viewId, string viewType, string viewName, DateTimeOffset dateCreated) {
			var viewIds = new List<Id<View>>() {
				viewId
			};

			var result = await _structureRepository.GetViews( viewIds );
			if( result.Any() ) {
				return result.First();
			}

			var view = await _structureRepository.AddView( viewId, viewType, viewName, dateCreated );

			await _userRepository.AddView( userId, viewId, dateCreated );

			return view;
		}
	}
}
