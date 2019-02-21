using System.Threading.Tasks;
using BlazorSpa.Client.Model;

namespace BlazorSpa.Client.Services {
	public interface ITokenService {
		Task<AuthorizationToken> GetToken( string code );

		Task<AuthorizationToken> RefreshToken( string refreshToken );
	}
}
