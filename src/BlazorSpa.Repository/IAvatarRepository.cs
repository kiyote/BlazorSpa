using System;
using System.Threading.Tasks;
using BlazorSpa.Repository.Model;

namespace BlazorSpa.Repository {
	public interface IAvatarRepository {
		Task<string> SetAvatar( Id<User> userId, string contentType, string content);

		Task<string> GetAvatarUrl( Id<User> userId );

		Task DeleteAvatar( Id<User> userId );
	}
}
