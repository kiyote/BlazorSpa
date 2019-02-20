using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorSpa.Client.Model;

namespace BlazorSpa.Client.Services {
	public interface IStructureApiService {

		Task<IEnumerable<View>> GetAllViews();

		Task<IEnumerable<View>> GetUserViews();

		Task<View> CreateView( string viewType, string viewName );

		Task<Structure> CreateStructureInView( string viewId, string structureType );

		Task<ApiStructureOperation> AddStructureToView( string structureId, string viewId );
	}
}
