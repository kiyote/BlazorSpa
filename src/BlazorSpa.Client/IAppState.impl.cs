using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BlazorSpa.Client {
	internal sealed class AppState : IAppState {

		private readonly IJSRuntime _js;

		public AppState( IJSRuntime jsRuntime ) {
			_js = jsRuntime;
		}

		public event EventHandler OnStateChanged;

		public async Task<string> GetUsername() {
			var value = await _js.InvokeAsync<string>( "appState.getItem", "Username" );
			return value ?? string.Empty;
		}

		public async Task SetUsername( string value ) {
			await _js.InvokeAsync<string>( "appState.setItem", "Username", value );
			OnStateChanged?.Invoke( this, EventArgs.Empty );
		}

		public async Task<string> GetAccessToken() {
			return await _js.InvokeAsync<string>( "appState.getItem", "AccessToken" );
		}

		public async Task SetAccessToken( string value ) {
			await _js.InvokeAsync<string>( "appState.setItem", "AccessToken", value );
		}

		public async Task<string> GetRefeshToken() {
			return await _js.InvokeAsync<string>( "appState.getItem", "RefreshToken" );
		}

		public async Task SetRefreshToken( string value ) {
			await _js.InvokeAsync<string>( "appState.setItem", "RefreshToken", value );
		}

		public async Task<DateTime> GetTokensExpireAt() {
			var value = await _js.InvokeAsync<string>( "appState.getItem", "TokensExpireAt" );
			if( value != default ) {
				return DateTime.Parse( value ).ToUniversalTime();
			}

			return DateTime.MinValue.ToUniversalTime();
		}

		public async Task SetTokensExpireAt( DateTime value ) {
			await _js.InvokeAsync<string>( "appState.setItem", "TokensExpireAt", value.ToString( "o" ) );
			OnStateChanged?.Invoke( this, EventArgs.Empty );
		}

		public async Task<bool> GetIsAuthenticated() {
			var tokensExpireAt = await GetTokensExpireAt();
			return tokensExpireAt > DateTime.UtcNow;
		}
	}
}
