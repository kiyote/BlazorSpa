using System;
using System.Threading.Tasks;
using BlazorSpa.Model;
using BlazorSpa.Repository;
using BlazorSpa.Repository.Model;

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

		public async Task<ApiUser> RecordLogin( string username ) {
			var authenticationInformation = await _authenticationRepository.GetUserInformation( username );
			var user = await _userRepository.GetByUsername( username );

			// If they don't have a local record, create one
			if( user == default ) {
				user = await _userRepository.AddUser(
					new Id<User>(),
					authenticationInformation.Username
				);
			}

			var avatarUrl = user.HasAvatar ? await _avatarRepository.GetAvatarUrl( user.Id ) : default;

			return new ApiUser(
				user.Id.Value,
				user.Name,
				avatarUrl);
		}

		public async Task<ApiUser> GetUser( string userId ) {
			var id = new Id<User>( userId );
			var user = await _userRepository.GetUser( id );
			var avatarUrl = user.HasAvatar ? await _avatarRepository.GetAvatarUrl( user.Id ) : default;

			return new ApiUser(
				user.Id.Value,
				user.Name,
				avatarUrl );
		}

		public async Task<string> SetAvatar( string userId, string contentType, string content ) {
			var id = new Id<User>( userId );
			var url = await _avatarRepository.SetAvatar( id, contentType, content );
			await _userRepository.UpdateAvatarStatus( id, true );
			var avatarUrl = await _avatarRepository.GetAvatarUrl( id );

			return avatarUrl;
		}
	}
}
