using System;
using System.Threading.Tasks;
using BlazorSpa.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Services;

namespace BlazorSpa.Client.Pages.Auth {
	public class ValidateComponent : ComponentBase {

		public static string Url = "/auth/validate";

		[Inject] private IUriHelper _uriHelper { get; set; }

		[Inject] private IAccessTokenProvider _accessTokenProvider { get; set; }

		[Inject] private IUserApiService _userApiService { get; set; }

		[Inject] private IAppState _state { get; set; }

		[Inject] private ITokenService _tokenService { get; set; }

		protected override async Task OnInitAsync() {
			var code = _uriHelper.GetParameter( "code" );

			var tokens = await _tokenService.GetToken( code );
			await _accessTokenProvider.SetTokens( tokens.access_token, tokens.refresh_token, DateTime.UtcNow.AddSeconds( tokens.expires_in ) );
			await _userApiService.RecordLogin();

			var userInfo = await _userApiService.GetUserInformation();
			await _state.SetUsername( userInfo.Name );
			_uriHelper.NavigateTo( "/" );
		}
	}
}
