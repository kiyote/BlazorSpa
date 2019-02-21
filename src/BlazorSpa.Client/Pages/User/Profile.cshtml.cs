using System;
using System.Globalization;
using System.Threading.Tasks;
using BlazorSpa.Client.Services;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.JSInterop;

namespace BlazorSpa.Client.Pages.User {
	public class ProfileComponent : BlazorComponent {

		public static string Url = "/user/profile";

		private Model.User _user;

		[Inject] private AppState State { get; set; }

		[Inject] private IUserApiService UserApiService { get; set; }

		public ElementRef FileUploadRef { get; set; }

		public bool Uploading { get; set; }

		public bool Changing { get; set; }

		public string UserId {
			get {
				return _user?.Id.Value;
			}
		}

		public string UserName {
			get {
				return _user?.Name;
			}
		}

		public string AvatarUrl { get; private set; }

		public string LastLogin {
			get {
				// We have to do this because right now Blazor has no concept
				// of the users culture info or timezone
				return _user.PreviousLogin
					?.Subtract( TimeSpan.FromHours( 5 ) )
					.ToString( "F", CultureInfo.GetCultureInfo( "en-US" ) )
					?? "None";
				//var displayDate = DateTimeOffset.Parse( response.PreviousLogin ).ToLocalTime().ToString( "F" );

			}
		}

		protected override async Task OnInitAsync() {
			await UpdateUserInformation();
		}

		private async Task UpdateUserInformation() {
			var response = await UserApiService.GetUserInformation();
			_user = response;
			AvatarUrl = _user.AvatarUrl;
		}

		public void ChangeAvatar() {
			Changing = true;
			StateHasChanged();
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

			/*
			using( var ms = new System.IO.MemoryStream( 10000 ) ) {
				using( var gzs = new System.IO.Compression.GZipStream( ms, System.IO.Compression.CompressionLevel.Optimal ) ) {
					var bytes = System.Text.Encoding.UTF8.GetBytes( "Content" );
					gzs.Write( bytes, 0, bytes.Length );
				}
			}
			*/
			
			if (mimeType.StartsWith("image")) {
				// Cheese it down to 64x64 for now
				//using( var image = Image.Load( Convert.FromBase64String( content ) ) ) {
				//	content = image.Clone( x => x.Resize( 64, 64 ) ).ToBase64String( ImageFormats.Png ).Split( ',' )[ 1 ];
				//	mimeType = "image/png";
				//}
				Uploading = true;
				StateHasChanged();
				AvatarUrl = await UserApiService.SetAvatar( mimeType, content );
				Uploading = false;
				Changing = false;
				StateHasChanged();
			}
		}
	}
}