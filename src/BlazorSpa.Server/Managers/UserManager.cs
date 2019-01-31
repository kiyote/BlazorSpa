using System;
using System.Threading.Tasks;
using BlazorSpa.Model;
using BlazorSpa.Repository;
using BlazorSpa.Repository.Model;
using BlazorSpa.Service;

namespace BlazorSpa.Server.Managers {
	public class UserManager {

		private readonly IIdentificationService _identificationService;
		private readonly IImageService _imageService;

		public UserManager(
			IIdentificationService identificationService,
			IImageService imageService
		) {
			_identificationService = identificationService;
			_imageService = imageService;
		}

		public async Task<ApiUser> RecordLogin( string username ) {
			var user = await _identificationService.RecordLogin( username );
			var avatarUrl = user.HasAvatar ? (await _imageService.Get( new Id<Image>( user.Id.Value ) ))?.Url : default;

			return ToApiUser( user, avatarUrl );
		}

		public async Task<ApiUser> GetUser( string userId ) {
			var id = new Id<User>( userId );
			var user = await _identificationService.GetUser( id );
			var avatarUrl = user.HasAvatar ? ( await _imageService.Get( new Id<Image>( user.Id.Value ) ) )?.Url : default;

			return ToApiUser( user, avatarUrl );
		}

		public async Task<string> SetAvatar( string userId, string contentType, string content ) {
			// Not a mistake, we're reusing the userId as the imageId for their avatar
			var id = new Id<Image>( userId );
			var url = await _imageService.Update( id, contentType, content );
			await _identificationService.SetAvatarStatus( new Id<User>( userId ), true );
			var avatar = await _imageService.Get( id );

			return avatar.Url;
		}

		private static ApiUser ToApiUser( User user, string avatarUrl ) {
			return new ApiUser(
				user.Id.Value,
				user.Name,
				avatarUrl,
				user.LastLogin.ToString( "O" ),
				user.PreviousLogin?.ToString( "O" ) ?? default );
		}
	}
}