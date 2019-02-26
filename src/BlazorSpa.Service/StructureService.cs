using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorSpa.Repository;
using BlazorSpa.Repository.Model;
using BlazorSpa.Shared;

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

		async Task<Structure> IStructureService.CreateStructure( 
			Id<Structure> structureId, 
			string structureType, 
			string name,
			DateTime createdOn 
		) {
			var structureIds = new List<Id<Structure>>() {
				structureId
			};
			var result = await _structureRepository.GetStructures( structureIds );
			if (result.Any()) {
				return result.First();
			}

			return await _structureRepository.AddStructure( structureId, structureType, name, createdOn );
		}

		async Task<StructureOperationStatus> IStructureService.AddStructureToView(
			Id<Structure> structureId, 
			Id<View> viewId, 
			DateTime createdOn 
		) {
			var structures = await _structureRepository.GetViewStructureIds( viewId );
			if (structures.Contains(structureId)) {
				return StructureOperationStatus.AlreadyExists;
			}

			return await _structureRepository.AddViewStructure( viewId, structureId, createdOn );
		}

		async Task<View> IStructureService.CreateViewWithUser(
			Id<User> userId, 
			Id<View> viewId, 
			string viewType, 
			string viewName, 
			DateTime createdOn 
		) {
			var viewIds = new List<Id<View>>() {
				viewId
			};

			var result = await _structureRepository.GetViews( viewIds );
			if( result.Any() ) {
				return result.First();
			}

			var view = await _structureRepository.AddView( viewId, viewType, viewName, createdOn );

			await _userRepository.AddView( userId, viewId, createdOn );

			return view;
		}

		async Task<IEnumerable<Structure>> IStructureService.GetChildStructures(Id<View> viewId, Id<Structure> structureId) {
			var structureIds = await _structureRepository.GetChildStructureIds( viewId, structureId );
			var structures = await _structureRepository.GetStructures( structureIds );

			return structures;
		}

		async Task IStructureService.AddChildStructure( 
			Id<View> viewId, 
			Id<Structure> parentStructureId, 
			Id<Structure> structureId, 
			DateTime createdOn 
		) {
			await _structureRepository.AddChildStructure( viewId, parentStructureId, structureId, createdOn.ToUniversalTime() );
		}

		async Task<Structure> IStructureService.GetParentStructure( Id<View> viewId, Id<Structure> structureId ) {
			var parentStructureId = await _structureRepository.GetParentStructureId( viewId, structureId );

			if (parentStructureId == default) {
				return default;
			}

			var structureIds = new List<Id<Structure>>() {
				parentStructureId
			};
			var structures = await _structureRepository.GetStructures( structureIds );
			return structures.FirstOrDefault();
		}
	}
}
