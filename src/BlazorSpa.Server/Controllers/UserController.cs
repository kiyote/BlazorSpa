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

		[HttpGet]
		public async Task<ActionResult<User>> GetUserInformation() {
			var result = await _userManager.GetUserInformation( _contextInformation.Username );

			if( result != default ) {
				return Ok( result );

			} else {
				return NotFound();
			}
		}
	}
}
