using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorSpa.Repository.Model;
using BlazorSpa.Service;
using BlazorSpa.Shared;
using ClientStructure = BlazorSpa.Client.Model.Structure;
using ClientView = BlazorSpa.Client.Model.View;

namespace BlazorSpa.Server.Managers {
	public class StructureManager {

		private readonly IStructureService _structureService;

		public StructureManager(
			IStructureService structureService
		) {
			_structureService = structureService;
		}

		public async Task<IEnumerable<ClientView>> GetUserViews( string userId ) {
			var views = await _structureService.GetUserViews( new Id<User>( userId ) );

			return views.Select( v => ToApiView( v ) );
		}

		public async Task<IEnumerable<ClientView>> GetAllViews() {
			var views = await _structureService.GetAllViews();

			return views.Select( v => ToApiView( v ) );
		}

		public async Task<IEnumerable<ClientStructure>> GetViewRootStructures( string viewId ) {
			var structures = await _structureService.GetViewRootStrutures( new Id<View>( viewId ) );

			return structures.Select( s => ToApiStructure( s ) );
		}

		public async Task<ClientView> CreateViewWithUser( string userId, string viewType, string viewName ) {
			var operationTimestamp = DateTime.UtcNow;
			var view = await _structureService.CreateViewWithUser( new Id<User>( userId ), new Id<View>(), viewType, viewName, operationTimestamp );

			return ToApiView( view );
		}

		public async Task<ClientStructure> CreateStructureInView( string viewId, string structureType, string name ) {
			var operationTimestamp = DateTime.UtcNow;
			var structure = await _structureService.CreateStructure( new Id<Structure>(), structureType, name, operationTimestamp );
			var result = await _structureService.AddStructureToView( structure.Id, new Id<View>( viewId ), operationTimestamp );

			if( result == Repository.StructureOperationStatus.Failure ) {
				return default;
			}

			return ToApiStructure( structure );
		}

		public async Task<IEnumerable<ClientStructure>> GetChildStructures( string viewId, string structureId ) {
			var structures = await _structureService.GetChildStructures( new Id<View>( viewId ), new Id<Structure>( structureId ) );

			return structures.Select( s => ToApiStructure( s ) );
		}

		public async Task<ClientStructure> CreateChildStructure( string viewId, string structureId, string structureType, string name ) {
			var operationTimestamp = DateTime.UtcNow;
			var newStructure = await _structureService.CreateStructure( new Id<Structure>(), structureType, name, operationTimestamp );
			await _structureService.AddChildStructure( new Id<View>( viewId ), new Id<Structure>( structureId ), newStructure.Id, operationTimestamp );

			return ToApiStructure( newStructure );
		}

		public async Task<ClientStructure> GetParentStructure( string viewId, string structureId ) {
			var structure = await _structureService.GetParentStructure( new Id<View>( viewId ), new Id<Structure>( structureId ) );
			return ToApiStructure( structure );
		}

		public async Task AddStructureToView( string viewId, string structureId ) {
			await _structureService.AddStructureToView(
				new Id<Structure>( structureId ),
				new Id<View>( viewId ),
				DateTime.UtcNow );
		}

		private ClientView ToApiView( View view ) {
			if (view == default) {
				return default;
			}

			return new ClientView(
				new Id<ClientView>( view.Id.Value ),
				view.ViewType,
				view.Name );
		}

		private ClientStructure ToApiStructure( Structure structure ) {
			if (structure == default) {
				return default;
			}

			return new ClientStructure(
				new Id<ClientStructure>( structure.Id.Value ),
				structure.StructureType,
				structure.Name );
		}
	}
}