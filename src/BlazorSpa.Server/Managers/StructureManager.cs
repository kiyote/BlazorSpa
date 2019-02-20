using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorSpa.Model;
using BlazorSpa.Repository.Model;
using BlazorSpa.Service;

namespace BlazorSpa.Server.Managers {
	public class StructureManager {

		private readonly IStructureService _structureService;

		public StructureManager(
			IStructureService structureService
		) {
			_structureService = structureService;
		}

		public async Task<IEnumerable<ApiView>> GetUserViews( string userId ) {
			var views = await _structureService.GetUserViews( new Id<User>( userId ) );

			return views.Select( v => ToApiView( v ) );
		}

		public async Task<IEnumerable<ApiView>> GetAllViews() {
			var views = await _structureService.GetAllViews();

			return views.Select( v => ToApiView( v ) );
		}

		public async Task<IEnumerable<ApiStructure>> GetViewRootStructures( string viewId ) {
			var structures = await _structureService.GetViewRootStrutures( new Id<View>( viewId ) );

			return structures.Select( s => ToApiStructure( s ) );
		}

		public async Task<ApiView> CreateViewWithUser( string userId, string viewType, string viewName ) {
			var operationTimestamp = DateTimeOffset.Now;
			var view = await _structureService.CreateViewWithUser( new Id<User>( userId ), new Id<View>(), viewType, viewName, operationTimestamp );

			return ToApiView( view );
		}

		public async Task<ApiStructure> CreateStructureInView( string viewId, string structureType ) {
			var operationTimestamp = DateTimeOffset.Now;
			var structure = await _structureService.CreateStructure( new Id<Structure>(), structureType, operationTimestamp );
			var result = await _structureService.AddStructureToView( structure.Id, new Id<View>( viewId ), operationTimestamp );

			if (result == Repository.StructureOperationStatus.Failure) {
				return default;
			}

			return ToApiStructure( structure );
		}

		public async Task AddStructureToView(string viewId, string structureId ) {
			await _structureService.AddStructureToView( new Id<Structure>( structureId ), new Id<View>( viewId ), DateTimeOffset.Now );
		}

		private ApiView ToApiView( View view ) {
			return new ApiView(
				view.Id.Value,
				view.ViewType,
				view.Name );
		}

		private ApiStructure ToApiStructure( Structure structure ) {
			return new ApiStructure(
				structure.Id.Value,
				structure.StructureType );
		}
	}
}