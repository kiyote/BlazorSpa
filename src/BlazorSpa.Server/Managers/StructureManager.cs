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

		private ApiView ToApiView(View view) {
			return new ApiView(
				view.Id.Value,
				view.Name );
		}

		private ApiStructure ToApiStructure(Structure structure) {
			return new ApiStructure(
				structure.Id.Value,
				structure.StructureType );
		}
	}
}
