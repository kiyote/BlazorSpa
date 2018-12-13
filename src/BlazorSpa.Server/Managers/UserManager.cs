using System;
using System.Threading.Tasks;
using BlazorSpa.Model;
using BlazorSpa.Repository;

namespace BlazorSpa.Server.Managers {
	public class UserManager {

		private readonly IAuthenticationRepository _authenticationRepository;
		private readonly IUserRepository _userRepository;

		public UserManager(
			IAuthenticationRepository authenticationRepository,
			IUserRepository userRepository
		) {
			_authenticationRepository = authenticationRepository;
			_userRepository = userRepository;
		}

		public async Task<User> GetUserInformation( string username ) {
			var authenticationInformation = await _authenticationRepository.GetUserInformation( username );
			var user = await _userRepository.GetByAuthenticationId( authenticationInformation.AuthenticationId );

			// If they don't have a local record, create one
			if( user == default ) {
				user = await _userRepository.AddUser(
					Guid.NewGuid().ToString( "N" ),
					authenticationInformation.AuthenticationId,
					authenticationInformation.Username
				);
			}

			return user;
		}
	}
}
