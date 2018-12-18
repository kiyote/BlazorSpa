using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpa.Repository {
	public interface IAvatarRepository {
		Task<string> SetAvatar(string userId, string contentType, string content);

		Task<string> GetAvatarUrl( string userId );
	}
}
