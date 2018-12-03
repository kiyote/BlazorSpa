using System;
using System.Threading.Tasks;
using BlazorSpa.Repository;

namespace BlazorSpa.Server.Managers {
	public class AuthenticationManager {

		private readonly IUserRepository _userRepository;

		public AuthenticationManager(
			IUserRepository userRepository
		) {
			_userRepository = userRepository;
		}

		public async Task<CreateUserStatus> Create( string username, string email ) {
			var status = await _userRepository.CreateUser( username, email );

			return status;
		}

		public async Task<AuthenticationResult> Authorize( string username, string password ) {
			return await _userRepository.Authenticate( username, password );
		}

		public async Task<bool> ForceChangePassword( string username, string newPassword, string session ) {
			return await _userRepository.ForceChangePassword( username, newPassword, session );
		}
	}
}