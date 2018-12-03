using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpa.Repository {
	public interface IUserRepository {
		Task<AuthenticationResult> Authenticate( string username, string password );

		Task<CreateUserStatus> CreateUser( string username, string email );

		Task<bool> DeleteUser( string username );

		Task<bool> ForceChangePassword( string username, string newPassword, string session );

		Task<UserInformationResult> GetUserInformation( string username );
	}
}
