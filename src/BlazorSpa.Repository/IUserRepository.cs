using System;
using System.Threading.Tasks;
using BlazorSpa.Repository.Model;

namespace BlazorSpa.Repository {
	public interface IUserRepository {

		Task<User> GetByUsername( string username );

		Task<User> AddUser( Id<User> userId, string username, DateTimeOffset lastLogin );

		Task<User> GetUser( Id<User> userId );

		Task<User> UpdateAvatarStatus( Id<User> userId, bool hasAvatar );

		Task<User> SetLastLogin( Id<User> userId, DateTimeOffset lastLogin );
	}
}
