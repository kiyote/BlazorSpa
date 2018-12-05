using System;
using System.Threading.Tasks;

namespace BlazorSpa.Repository {
	public interface IAuthenticationRepository {

		Task<UserInformationResult> GetUserInformation( string username );
	}
}
