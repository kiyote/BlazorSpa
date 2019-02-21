using System;
using System.Threading.Tasks;

namespace BlazorSpa.Client.Services {
	public interface IAccessTokenProvider {

		void SetTokens( string accessToken, string refreshToken, DateTime expiresAt );

		Task<string> GetJwtToken();
	}
}
