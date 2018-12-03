using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorSpa.Model.Api;

namespace BlazorSpa.Client.Services {
	public interface ITokenService {
		Task<AuthorizationToken> GetToken( string code );

		Task<AuthorizationToken> RefreshToken( string refreshToken );
	}
}
