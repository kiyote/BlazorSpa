using System;
using System.Threading.Tasks;
using BlazorSpa.Model;
using BlazorSpa.Repository.Model;
using BlazorSpa.Service;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;
using Image = BlazorSpa.Repository.Model.Image;
using LaborImage = SixLabors.ImageSharp.Image;

namespace BlazorSpa.Server.Managers {
	public sealed class UserManager {

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

			return ToApiUser( user, await GetAvatarUrl( user ) );
		}

		public async Task<ApiUser> GetUser( string userId ) {
			var id = new Id<User>( userId );
			var user = await _identificationService.GetUser( id );

			return ToApiUser( user, await GetAvatarUrl( user ) );
		}

		public async Task<string> SetAvatar( string userId, string contentType, string content ) {
			using( var image = LaborImage.Load( Convert.FromBase64String( content ) ) ) {
				if( ( image.Width != 64 ) || ( image.Height != 64 ) ) {
					content = image.Clone( x => x.Resize( 64, 64 ) ).ToBase64String( PngFormat.Instance ).Split( ',' )[ 1 ];
					contentType = "image/png";
				}
			}

			// Not a mistake, we're reusing the userId as the imageId for their avatar
			var id = new Id<Image>( userId );
			var avatar = await _imageService.Update( id, contentType, content );
			await _identificationService.SetAvatarStatus( new Id<User>( userId ), true );

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

		private async Task<string> GetAvatarUrl( User user ) {
			string result = default;

			if( user.HasAvatar ) {
				result = ( await _imageService.Get( new Id<Image>( user.Id.Value ) ) )?.Url;
			}

			return result;
		}
	}
}