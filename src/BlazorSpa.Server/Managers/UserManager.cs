using System;
using System.Threading.Tasks;
using BlazorSpa.Repository.Model;
using BlazorSpa.Service;
using BlazorSpa.Shared;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.Primitives;
using Image = BlazorSpa.Repository.Model.Image;
using LaborImage = SixLabors.ImageSharp.Image;
using ClientUser = BlazorSpa.Client.Model.User;

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

		public async Task<ClientUser> RecordLogin( string username ) {
			var user = await _identificationService.RecordLogin( username );

			return ToApiUser( user, await GetAvatarUrl( user ) );
		}

		public async Task<ClientUser> GetUser( string userId ) {
			var id = new Id<User>( userId );
			var user = await _identificationService.GetUser( id );

			return ToApiUser( user, await GetAvatarUrl( user ) );
		}

		public async Task<string> SetAvatar( string userId, string contentType, string content ) {
			using( var image = LaborImage.Load( Convert.FromBase64String( content ) ) ) {
				if( ( image.Width != 64 ) || ( image.Height != 64 ) ) {
					var options = new ResizeOptions() {
						Mode = ResizeMode.Max,
						Size = new Size( 64, 64 )
					};
					content = image.Clone( x => x.Resize( options ) ).ToBase64String( PngFormat.Instance ).Split( ',' )[ 1 ];
					contentType = "image/png";
				}
			}

			// Not a mistake, we're reusing the userId as the imageId for their avatar
			var id = new Id<Image>( userId );
			var avatar = await _imageService.Update( id, contentType, content );
			await _identificationService.SetAvatarStatus( new Id<User>( userId ), true );

			return avatar.Url;
		}

		private static ClientUser ToApiUser( User user, string avatarUrl ) {
			return new ClientUser(
				new Id<ClientUser>(user.Id.Value),
				user.Name,
				avatarUrl,
				user.LastLogin,
				user.PreviousLogin
			);
		}

		private async Task<string> GetAvatarUrl( User user ) {
			string result = default;

			if( user.HasAvatar ) {
				// Not a mistake, we're reusing the userId as the imageId for their avatar
				result = ( await _imageService.Get( new Id<Image>( user.Id.Value ) ) )?.Url;
			}

			return result;
		}
	}
}