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

		public async Task<UserInformationResult> GetUserInformation(string username) {
			var authenticationInformation = await _authenticationRepository.GetUserInformation( username );
			var user = await _userRepository.GetByAuthenticationId( authenticationInformation.UserId );

			// If they don't have a local record, create one
			if (user == default) {
				user = await _userRepository.AddUser( new Id<User>(), authenticationInformation.UserId );
			}

			return new UserInformationResult() {
				UserId = user.Id.Value,
				Email = authenticationInformation.Email,
				Username = authenticationInformation.Username
			};

		}
	}
}
