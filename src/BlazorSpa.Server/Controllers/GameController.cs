using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorSpa.Model.Api;
using Microsoft.AspNetCore.Mvc;


namespace BlazorSpa.Server.Controllers {
	public class GameController : Controller {

		public GameController() {

		}

		[HttpGet]
		public async Task<ActionResult<GamesListResponse>> GetGames() {

			return default;
		}
	}
}
