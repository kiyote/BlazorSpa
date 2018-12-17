using System;
using System.Threading.Tasks;
using BlazorSpa.Client.Services;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace BlazorSpa.Client.Pages.User {
	public class ProfileComponent : BlazorComponent {

		public static string Url = "/user/profile";

		[Inject] private AppState State { get; set; }

		[Inject] private IUserApiService UserApiService { get; set; }

		public string UserId { get; set; }

		public string Username { get; set; }

		public ElementRef FileUploadRef { get; set; }

		protected override async Task OnInitAsync() {
			var response = await UserApiService.GetUserInformation();
			UserId = response.Id;
			Username = response.Name;
		}

		public async Task UploadFile() {
			var data = await JSRuntime.Current.InvokeAsync<string>( "profileFiles.readUploadedFileAsText", FileUploadRef );
			if (!data.StartsWith("data:")) {
				// No idea what to do here, the browser is behaving strangely...
				return;
			}
			data = data.Substring( "data:".Length );
			var parts = data.Split( ',' );
			var descriptor = parts[ 0 ].Split( ';' );
			var mimeType = descriptor[ 0 ];
			var encoding = descriptor[ 1 ];
			var content = parts[ 1 ];
			Console.WriteLine( mimeType );
			Console.WriteLine( encoding );
		}
	}
}