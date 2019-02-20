using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorSpa.Model;
using BlazorSpa.Server.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorSpa.Server.Controllers {
	[Authorize]
	[Route( "api/structure" )]
	[Produces("application/json")]
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

		[HttpGet( "views" )]
		public async Task<ActionResult<IEnumerable<ApiView>>> GetAllViews() {
			var result = await _structureManager.GetAllViews();

			if( result != default ) {
				return Ok( result );

			} else {
				return NotFound();
			}
		}

		[HttpGet( "view" )]
		public async Task<ActionResult<IEnumerable<ApiView>>> GetUserViews() {
			var userId = _contextInformation.UserId;
			var result = await _structureManager.GetUserViews( userId );

			return Ok( result );
		}

		[HttpPost( "view" )]
		public async Task<ActionResult<ApiView>> CreateView( [FromBody] ApiView view ) {

			if( ( view == default )
				|| ( string.IsNullOrWhiteSpace( view.ViewType ) )
				|| ( string.IsNullOrWhiteSpace( view.Name ) ) ) {
				return BadRequest();
			}
			var userId = _contextInformation.UserId;

			var result = await _structureManager.CreateViewWithUser( userId, view.ViewType, view.Name );
			return Ok( result );
		}

		[HttpPost( "view/{viewId}" )]
		public async Task<ActionResult<ApiStructure>> CreateStructureInView( string viewId, [FromBody] ApiStructure structure ) {
			if ((structure == default)
				|| (string.IsNullOrWhiteSpace(structure.StructureType))
				|| string.IsNullOrWhiteSpace(viewId)) {
				return BadRequest();
			}

			var result = await _structureManager.CreateStructureInView( viewId, structure.StructureType );

			if (result != default) {
				return Ok( result );
			}

			return StatusCode( StatusCodes.Status500InternalServerError );
		}

		[HttpPost( "{structureId}/view/{viewId}" )]
		public async Task<ActionResult<ApiStructure>> AddStructureToView( string viewId, string structureId ) {
			if( ( string.IsNullOrWhiteSpace( structureId ) )
				|| string.IsNullOrWhiteSpace( viewId ) ) {
				return BadRequest();
			}

			await _structureManager.AddStructureToView( viewId, structureId );
			return Ok();
		}
	}
}