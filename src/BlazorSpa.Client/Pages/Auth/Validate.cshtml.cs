using System;
using System.Threading.Tasks;
using BlazorSpa.Client.Services;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Services;

namespace BlazorSpa.Client.Pages.Auth {
	public class ValidateComponent : BlazorComponent {

		public static string Url = "/auth/validate";

		[Inject] private IUriHelper _uriHelper { get; set; }

		[Inject] private IAccessTokenProvider _accessTokenProvider { get; set; }

		[Inject] private IUserApiService _userApiService { get; set; }

		[Inject] private AppState _state { get; set; }

		[Inject] private ITokenService _tokenService { get; set; }

		protected override async Task OnInitAsync() {
			var code = _uriHelper.GetParameter( "code" );

			var tokens = await _tokenService.GetToken( code );
			_accessTokenProvider.SetTokens( tokens.access_token, tokens.refresh_token, DateTimeOffset.Now.AddSeconds( tokens.expires_in ) );
			var userInfo = await _userApiService.GetUserInformation();
			_state.Username = userInfo.Name;
			_uriHelper.NavigateTo( "/" );
		}
	}
}
