using System;
using System.Threading.Tasks;
using BlazorSpa.Model;
using BlazorSpa.Server.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorSpa.Server.Controllers {
	[Authorize]
	[Route( "api/structure" )]
	public sealed class StructureController : Controller {

		private readonly StructureManager _structureManager;
		private readonly IContextInformation _contextInformation;

		public StructureController(
			StructureManager structureManager,
			IContextInformation contextInformation
		) {
			_structureManager = structureManager;
			_contextInformation = contextInformation;
		}

		[HttpGet("views")]
		public async Task<ActionResult<ApiView>> GetAllViews() {
			var result = await _structureManager.GetAllViews();

			if( result != default ) {
				return Ok( result );

			} else {
				return NotFound();
			}
		}
	}
}