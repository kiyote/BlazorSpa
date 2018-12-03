using System;
using System.Threading.Tasks;

namespace BlazorSpa.Client.Services {
	internal sealed class AccessTokenProvider : IAccessTokenProvider {

		private readonly AppState _state;
		private readonly ITokenService _tokenService;

		public AccessTokenProvider(
			AppState state,
			ITokenService tokenService
		) {
			_state = state;
			_tokenService = tokenService;
			_state.TokensExpireAt = DateTimeOffset.Now;
		}

		public void SetTokens( string accessToken, string refreshToken, DateTimeOffset expiresAt ) {
			_state.AccessToken = accessToken;
			_state.RefreshToken = refreshToken;
			_state.TokensExpireAt = expiresAt;			
		}

		async Task<string> IAccessTokenProvider.GetJwtToken() {
			if (_state.TokensExpireAt < DateTimeOffset.Now) {
				var tokens = await _tokenService.RefreshToken( _state.AccessToken );
				SetTokens( tokens.access_token, tokens.refresh_token, DateTimeOffset.Now.AddSeconds( tokens.expires_in ) );
			}
			return _state.AccessToken;
		}
	}
}
