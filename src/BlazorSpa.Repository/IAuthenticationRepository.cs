using System;
using System.Threading.Tasks;
using BlazorSpa.Model;

namespace BlazorSpa.Repository {
	public interface IAuthenticationRepository {

		Task<AuthenticationUserInformation> GetUserInformation( string username );
	}
}
