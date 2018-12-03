using System;
using System.Threading.Tasks;
using BlazorSpa.Client.Services;
using BlazorSpa.Model.Api;
using Microsoft.AspNetCore.Blazor.Components;

namespace BlazorSpa.Client.Pages.User {
	public class ProfileComponent : BlazorComponent {

		public static string Url = "/user/profile";

		[Inject] private AppState State { get; set; }

		[Inject] private IUserApiService UserApiService { get; set; }

		public string UserId { get; set; }

		public string Username { get; set; }

		public string Email { get; set; }

		protected override async Task OnInitAsync() {
			var response = await UserApiService.GetUserInformation( new UserInformationRequest() { Username = State.Username } );
			UserId = response.UserId;
			Username = response.Username;
			Email = response.Email;
		}
	}
}