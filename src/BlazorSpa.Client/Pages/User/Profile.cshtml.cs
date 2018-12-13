using System;
using System.Threading.Tasks;
using BlazorSpa.Client.Services;
using Microsoft.AspNetCore.Blazor.Components;
using Newtonsoft.Json;

namespace BlazorSpa.Client.Pages.User {
	public class ProfileComponent : BlazorComponent {

		public static string Url = "/user/profile";

		[Inject] private AppState State { get; set; }

		[Inject] private IUserApiService UserApiService { get; set; }

		public string UserId { get; set; }

		public string Username { get; set; }

		protected override async Task OnInitAsync() {
			var response = await UserApiService.GetUserInformation();
			Console.WriteLine( JsonConvert.SerializeObject(response) );
			UserId = response.Id;
			Username = response.Name;
		}
	}
}