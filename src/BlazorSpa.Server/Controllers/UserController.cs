using System;
using System.Threading.Tasks;
using BlazorSpa.Model;
using BlazorSpa.Server.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorSpa.Server.Controllers {
	[Authorize]
	[Route( "api/user" )]
	public class UserController : Controller {

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
		public async Task<ActionResult<ApiUser>> GetUserInformation() {
			var result = await _userManager.GetUser( _contextInformation.UserId );

			if( result != default ) {
				return Ok( result );

			} else {
				return NotFound();
			}
		}

		[HttpPost( "avatar" )]
		public async Task<ActionResult<string>> SetAvatar( [FromBody] SetAvatarRequest request ) {

			var url = await _userManager.SetAvatar( _contextInformation.UserId, request.ContentType, request.Content );
			return Ok( new SetAvatarResponse() { Url = url } );
		}
	}
}