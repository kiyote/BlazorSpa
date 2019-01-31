using System;
using System.Threading.Tasks;
using BlazorSpa.Repository;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;

namespace BlazorSpa.Service {
	public sealed class ImageService : IImageService {

		private readonly IImageRepository _imageRepository;

		public ImageService(
			IImageRepository imageRepository
		) {
			_imageRepository = imageRepository;
		}

		async Task<Repository.Model.Image> IImageService.Add( string contentType, string content ) {
			using( var image = Image.Load( Convert.FromBase64String( content ) ) ) {
				if( ( image.Width != 64 ) || ( image.Height != 64 ) ) {					
					content = image.Clone( x => x.Resize( 64, 64 ) ).ToBase64String( PngFormat.Instance ).Split( ',' )[ 1 ];
					contentType = "image/png";
				}
			}

			var id = new Repository.Model.Id<Repository.Model.Image>();
			return await _imageRepository.Add( id, contentType, content );
		}

		async Task<Repository.Model.Image> IImageService.Update( Repository.Model.Id<Repository.Model.Image> id, string contentType, string content ) {
			using( var image = Image.Load( Convert.FromBase64String( content ) ) ) {
				if( ( image.Width != 64 ) || ( image.Height != 64 ) ) {
					content = image.Clone( x => x.Resize( 64, 64 ) ).ToBase64String( PngFormat.Instance ).Split( ',' )[ 1 ];
					contentType = "image/png";
				}
			}

			return await _imageRepository.Update( id, contentType, content );
		}

		async Task<Repository.Model.Image> IImageService.Get(Repository.Model.Id<Repository.Model.Image> id) {
			return await _imageRepository.Get( id );
		}
	}
}
