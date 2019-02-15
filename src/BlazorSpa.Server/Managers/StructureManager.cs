using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

		public async Task<IEnumerable<View>> GetAllViews() {
			return await _structureService.GetAllViews();
		}
	}
}
