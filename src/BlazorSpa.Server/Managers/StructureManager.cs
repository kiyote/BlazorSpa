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

		public async Task<ClientStructure> CreateStructureInView( string viewId, string structureType ) {
			var operationTimestamp = DateTime.UtcNow;
			var structure = await _structureService.CreateStructure( new Id<Structure>(), structureType, operationTimestamp );
			var result = await _structureService.AddStructureToView( structure.Id, new Id<View>( viewId ), operationTimestamp );

			if (result == Repository.StructureOperationStatus.Failure) {
				return default;
			}

			return ToApiStructure( structure );
		}

		public async Task AddStructureToView(string viewId, string structureId ) {
			await _structureService.AddStructureToView( 
				new Id<Structure>( structureId ), 
				new Id<View>( viewId ), 
				DateTime.UtcNow );
		}

		private ClientView ToApiView( View view ) {
			return new ClientView(
				view.Id.Value,
				view.ViewType,
				view.Name );
		}

		private ClientStructure ToApiStructure( Structure structure ) {
			return new ClientStructure(
				structure.Id.Value,
				structure.StructureType );
		}
	}
}