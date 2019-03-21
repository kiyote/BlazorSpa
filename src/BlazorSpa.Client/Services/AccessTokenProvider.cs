using System;
using System.Threading.Tasks;

namespace BlazorSpa.Client.Services {
	internal sealed class AccessTokenProvider : IAccessTokenProvider {

		private readonly IAppState _state;
		private readonly ITokenService _tokenService;

		public AccessTokenProvider(
			IAppState state,
			ITokenService tokenService
		) {
			_state = state;
			_tokenService = tokenService;
		}

		public async Task SetTokens( string accessToken, string refreshToken, DateTime expiresAt ) {
			await _state.SetAccessToken( accessToken );
			await _state.SetRefreshToken( refreshToken );
			await _state.SetTokensExpireAt( expiresAt );
		}

		async Task<string> IAccessTokenProvider.GetJwtToken() {
			if( await _state.GetTokensExpireAt() < DateTimeOffset.Now ) {
				var tokens = await _tokenService.RefreshToken( await _state.GetAccessToken() );
				if( tokens != default ) {
					await SetTokens( tokens.access_token, tokens.refresh_token, DateTime.UtcNow.AddSeconds( tokens.expires_in ) );
				}
			}
			return await _state.GetAccessToken();
		}
	}
}
