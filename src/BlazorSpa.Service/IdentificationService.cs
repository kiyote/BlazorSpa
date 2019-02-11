using System;
using System.Threading.Tasks;
using BlazorSpa.Repository;
using BlazorSpa.Repository.Model;

namespace BlazorSpa.Service {
	internal sealed class IdentificationService: IIdentificationService {

		private readonly IAuthenticationRepository _authenticationRepository;
		private readonly IUserRepository _userRepository;

		public IdentificationService(
			IAuthenticationRepository authenticationRepository,
			IUserRepository userRepository
		) {
			_authenticationRepository = authenticationRepository;
			_userRepository = userRepository;
		}

		async Task<User> IIdentificationService.RecordLogin( string username ) {
			var authenticationInformation = await _authenticationRepository.GetUserInformation( username );
			var user = await _userRepository.GetByUsername( username );
			DateTimeOffset lastLogin = DateTimeOffset.Now;

			// If they don't have a local record, create one
			if( user == default ) {
				user = await _userRepository.AddUser(
					new Id<User>(),
					authenticationInformation.Username,
					lastLogin
				);
			} else {
				user = await _userRepository.SetLastLogin( user.Id, lastLogin );
			}

			return user;
		}

		async Task<User> IIdentificationService.GetUser(Id<User> userId) {
			return await _userRepository.GetUser( userId );
		}

		async Task<User> IIdentificationService.SetAvatarStatus(Id<User> userId, bool hasAvatar) {
			return await _userRepository.UpdateAvatarStatus( userId, hasAvatar );
		}
	}
}
