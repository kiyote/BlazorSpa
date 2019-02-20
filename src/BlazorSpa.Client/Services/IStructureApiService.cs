using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorSpa.Model;

namespace BlazorSpa.Client.Services {
	public interface IStructureApiService {

		Task<IEnumerable<ApiView>> GetAllViews();

		Task<IEnumerable<ApiView>> GetUserViews();

		Task<ApiView> CreateView( string viewType, string viewName );

		Task<ApiStructure> CreateStructureInView( string viewId, string structureType );

		Task<ApiStructureOperation> AddStructureToView( string structureId, string viewId );
	}
}
