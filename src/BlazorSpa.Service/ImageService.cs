using System;
using System.Threading.Tasks;
using BlazorSpa.Repository;

namespace BlazorSpa.Service {
	public sealed class ImageService : IImageService {

		private readonly IImageRepository _imageRepository;

		public ImageService(
			IImageRepository imageRepository
		) {
			_imageRepository = imageRepository;
		}

		async Task<Repository.Model.Image> IImageService.Add( string contentType, string content ) {

			var id = new Repository.Model.Id<Repository.Model.Image>();
			return await _imageRepository.Add( id, contentType, content );
		}

		async Task<Repository.Model.Image> IImageService.Update( Repository.Model.Id<Repository.Model.Image> id, string contentType, string content ) {
			return await _imageRepository.Update( id, contentType, content );
		}

		async Task<Repository.Model.Image> IImageService.Get(Repository.Model.Id<Repository.Model.Image> id) {
			return await _imageRepository.Get( id );
		}
	}
}
