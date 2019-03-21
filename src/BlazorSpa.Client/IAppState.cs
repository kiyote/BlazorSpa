using System;
using System.Threading.Tasks;

namespace BlazorSpa.Client {
	public interface IAppState {
		event EventHandler OnStateChanged;

		Task<string> GetAccessToken();
		Task<bool> GetIsAuthenticated();
		Task<string> GetRefeshToken();
		Task<DateTime> GetTokensExpireAt();
		Task<string> GetUsername();
		Task SetAccessToken( string value );
		Task SetRefreshToken( string value );
		Task SetTokensExpireAt( DateTime value );
		Task SetUsername( string value );
	}
}