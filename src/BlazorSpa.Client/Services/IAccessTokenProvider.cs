using System;
using System.Threading.Tasks;

namespace BlazorSpa.Client.Services {
	public interface IAccessTokenProvider {

		Task SetTokens( string accessToken, string refreshToken, DateTime expiresAt );

		Task<string> GetJwtToken();
	}
}
