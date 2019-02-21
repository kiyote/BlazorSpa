using System.Threading.Tasks;
using BlazorSpa.Repository;
using BlazorSpa.Repository.Model;
using BlazorSpa.Shared;

namespace BlazorSpa.Service {
	internal sealed class ImageService : IImageService {

		private readonly IImageRepository _imageRepository;

		public ImageService(
			IImageRepository imageRepository
		) {
			_imageRepository = imageRepository;
		}

		async Task<Image> IImageService.Add( string contentType, string content ) {

			var id = new Id<Image>();
			return await _imageRepository.Add( id, contentType, content );
		}

		async Task<Image> IImageService.Update( Id<Image> id, string contentType, string content ) {
			return await _imageRepository.Update( id, contentType, content );
		}

		async Task<Image> IImageService.Get(Id<Image> id) {
			return await _imageRepository.Get( id );
		}
	}
}
