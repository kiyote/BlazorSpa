using System;
using System.Threading.Tasks;
using BlazorSpa.Model;
using BlazorSpa.Repository;

namespace BlazorSpa.Server.Managers {
	public class UserManager {

		private readonly IAuthenticationRepository _authenticationRepository;
		private readonly IUserRepository _userRepository;
		private readonly IAvatarRepository _avatarRepository;

		public UserManager(
			IAuthenticationRepository authenticationRepository,
			IUserRepository userRepository,
			IAvatarRepository avatarRepository
		) {
			_authenticationRepository = authenticationRepository;
			_userRepository = userRepository;
			_avatarRepository = avatarRepository;
		}

		public async Task<User> RecordLogin( string username ) {
			var authenticationInformation = await _authenticationRepository.GetUserInformation( username );
			var user = await _userRepository.GetByUsername( username );

			// If they don't have a local record, create one
			if( user == default ) {
				user = await _userRepository.AddUser(
					User.CreateId(),
					authenticationInformation.Username
				);
			}

			var avatarUrl = user.HasAvatar ? await _avatarRepository.GetAvatarUrl( user.Id ) : default;

			return new User(
				user.Id,
				user.Name,
				avatarUrl);
		}

		public async Task<User> GetUser( string userId ) {
			var user = await _userRepository.GetUser( userId );
			var avatarUrl = user.HasAvatar ? await _avatarRepository.GetAvatarUrl( user.Id ) : default;

			return new User(
				user.Id,
				user.Name,
				avatarUrl );
		}

		public async Task<string> SetAvatar( string userId, string contentType, string content ) {
			var url = await _avatarRepository.SetAvatar( userId, contentType, content );
			await _userRepository.UpdateAvatarStatus( userId, true );
			var avatarUrl = await _avatarRepository.GetAvatarUrl( userId );

			return avatarUrl;
		}
	}
}
