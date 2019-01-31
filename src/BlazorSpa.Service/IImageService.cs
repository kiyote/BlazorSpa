using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlazorSpa.Repository.Model;

namespace BlazorSpa.Service {
	public interface IImageService {
		Task<Image> Add( string contentType, string content );

		Task<Image> Update( Id<Image> id, string contentType, string content );

		Task<Image> Get( Id<Image> id );
	}
}
