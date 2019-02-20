using System;
using System.Threading.Tasks;
using BlazorSpa.Shared;
using BlazorSpa.Repository.Model;

namespace BlazorSpa.Repository {
	public interface IImageRepository {
		Task<Image> Add( Id<Image> id, string contentType, string content );

		Task<Image> Update( Id<Image> id, string contentType, string content );

		Task Remove( Id<Image> id );

		Task<Image> Get( Id<Image> id );

		Task<bool> Exists( Id<Image> id );
	}
}
