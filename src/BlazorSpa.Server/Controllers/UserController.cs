using System.Threading.Tasks;
using BlazorSpa.Client.Model;
using BlazorSpa.Model;
using BlazorSpa.Server.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorSpa.Server.Controllers {
	[Authorize]
	[Route( "api/user" )]
	public sealed class UserController : Controller {

		private readonly UserManager _userManager;
		private readonly IContextInformation _contextInformation;

		public UserController(
			UserManager userManager,
			IContextInformation contextInformation
		) {
			_userManager = userManager;
			_contextInformation = contextInformation;
		}

		[HttpGet( "login" )]
		public async Task<ActionResult> RecordLogin() {
			var user = await _userManager.RecordLogin( _contextInformation.Username );

			return Ok( user );
		}

		[HttpGet]
		public async Task<ActionResult<User>> GetUserInformation() {
			var result = await _userManager.GetUser( _contextInformation.UserId );

			if( result != default ) {
				return Ok( result );

			} else {
				return NotFound();
			}
		}

		[HttpPost( "avatar" )]
		public async Task<ActionResult<AvatarUrl>> SetAvatar( [FromBody] AvatarImage request ) {

			var url = await _userManager.SetAvatar( _contextInformation.UserId, request.ContentType, request.Content );
			return Ok( new AvatarUrl( url ) );
		}
	}
}