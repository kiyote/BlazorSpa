using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorSpa.Model;

namespace BlazorSpa.Client.Services {
	public interface IStructureApiService {

		Task<IEnumerable<ApiView>> GetViews();
	}
}
