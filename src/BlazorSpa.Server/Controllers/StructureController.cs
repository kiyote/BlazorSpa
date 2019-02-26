using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorSpa.Client.Model;
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
		public async Task<ActionResult<IEnumerable<View>>> GetAllViews() {
			var result = await _structureManager.GetAllViews();

			if( result != default ) {
				return Ok( result );

			} else {
				return NotFound();
			}
		}

		[HttpGet( "view" )]
		public async Task<ActionResult<IEnumerable<View>>> GetUserViews() {
			var userId = _contextInformation.UserId;
			var result = await _structureManager.GetUserViews( userId );

			return Ok( result );
		}

		[HttpPost( "view" )]
		public async Task<ActionResult<View>> CreateView( [FromBody] View view ) {

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
		public async Task<ActionResult<Structure>> CreateStructureInView( string viewId, [FromBody] Structure structure ) {
			if ((structure == default)
				|| (string.IsNullOrWhiteSpace(structure.StructureType))
				|| string.IsNullOrWhiteSpace(viewId)) {
				return BadRequest();
			}

			var result = await _structureManager.CreateStructureInView( viewId, structure.StructureType, structure.Name );

			if (result != default) {
				return Ok( result );
			}

			return StatusCode( StatusCodes.Status500InternalServerError );
		}

		[HttpGet( "view/{viewId}/structure/{structureId}" )]
		public async Task<ActionResult<IEnumerable<Structure>>> GetChildStructures( string viewId, string structureId ) {
			if( ( string.IsNullOrWhiteSpace( structureId ) )
				|| string.IsNullOrWhiteSpace( viewId ) ) {
				return BadRequest();
			}


			return Ok( await _structureManager.GetChildStructures( viewId, structureId ) );
		}

		[HttpPost( "view/{viewId}/structure/{structureId}" )]
		public async Task<ActionResult<Structure>> CreateChildStructure( string viewId, string structureId, [FromBody] Structure structure ) {
			if( ( string.IsNullOrWhiteSpace( structureId ) )
				|| string.IsNullOrWhiteSpace( viewId )
				|| structure == default ) {
				return BadRequest();
			}


			return Ok( await _structureManager.CreateChildStructure( viewId, structureId, structure.StructureType, structure.Name ) );
		}

		[HttpGet("view/{viewId}/structure")]
		public async Task<ActionResult<IEnumerable<Structure>>> GetViewStructures(string viewId) {
			if (string.IsNullOrWhiteSpace(viewId)) {
				return BadRequest();
			}

			return Ok( await _structureManager.GetViewRootStructures( viewId ) );
		}

		[HttpPost( "{structureId}/view/{viewId}" )]
		public async Task<ActionResult<Structure>> AddStructureToView( string viewId, string structureId ) {
			if( ( string.IsNullOrWhiteSpace( structureId ) )
				|| string.IsNullOrWhiteSpace( viewId ) ) {
				return BadRequest();
			}

			await _structureManager.AddStructureToView( viewId, structureId );
			return Ok();
		}

		[HttpGet( "{structureId}/view/{viewId}" )]
		public async Task<ActionResult<Structure>> GetParentStructure( string viewId, string structureId ) {
			if( ( string.IsNullOrWhiteSpace( structureId ) )
				|| string.IsNullOrWhiteSpace( viewId ) ) {
				return BadRequest();
			}
			
			return Ok( await _structureManager.GetParentStructure( viewId, structureId ));
		}

	}
}