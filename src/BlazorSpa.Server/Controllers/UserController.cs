using System;
using System.Threading.Tasks;
using BlazorSpa.Model.Api;
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
		public async Task<ActionResult<UserInformationResponse>> GetUserInformation() {
			return await GetUserInformation( string.Empty );
		}

		[HttpGet("{username}")]
		public async Task<ActionResult<UserInformationResponse>> GetUserInformation(string username) {
			string loggedInUsername = _contextInformation.Username;

			if (string.IsNullOrWhiteSpace(username)) {
				username = loggedInUsername;
			}
			var result = await _userManager.GetUserInformation( username );

			if (result != default) {
				return Ok( new UserInformationResponse() {
					UserId = result.UserId,
					Username = result.Username,
					Email = result.Email
				} );

			} else {
				return NotFound();
			}
		}
	}
}
