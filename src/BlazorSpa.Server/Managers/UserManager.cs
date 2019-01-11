using System;
using System.Threading.Tasks;
using BlazorSpa.Model;
using BlazorSpa.Repository;
using BlazorSpa.Repository.Model;
using BlazorSpa.Service;

namespace BlazorSpa.Server.Managers {
	public class UserManager {

		private readonly IIdentificationService _identificationService;
		private readonly IAvatarRepository _avatarRepository;

		public UserManager(
			IIdentificationService identificationService,
			IAvatarRepository avatarRepository
		) {
			_identificationService = identificationService;
			_avatarRepository = avatarRepository;
		}

		public async Task<ApiUser> RecordLogin( string username ) {
			var user = await _identificationService.RecordLogin( username );
			var avatarUrl = user.HasAvatar ? await _avatarRepository.GetAvatarUrl( user.Id ) : default;

			return ToApiUser( user, avatarUrl );
		}

		public async Task<ApiUser> GetUser( string userId ) {
			var id = new Id<User>( userId );
			var user = await _identificationService.GetUser( id );
			var avatarUrl = user.HasAvatar ? await _avatarRepository.GetAvatarUrl( user.Id ) : default;

			return ToApiUser( user, avatarUrl );
		}

		public async Task<string> SetAvatar( string userId, string contentType, string content ) {
			var id = new Id<User>( userId );
			var url = await _avatarRepository.SetAvatar( id, contentType, content );
			await _identificationService.SetAvatarStatus( id, true );
			var avatarUrl = await _avatarRepository.GetAvatarUrl( id );

			return avatarUrl;
		}

		private static ApiUser ToApiUser(User user, string avatarUrl) {
			return new ApiUser(
				user.Id.Value,
				user.Name,
				avatarUrl,
				user.LastLogin.ToString( "O" ),
				user.PreviousLogin?.ToString( "O" ) ?? default );
		}
	}
}