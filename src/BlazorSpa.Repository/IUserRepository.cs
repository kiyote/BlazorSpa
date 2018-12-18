using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlazorSpa.Repository.Model;

namespace BlazorSpa.Repository {
	public interface IUserRepository {

		Task<User> GetByUsername( string username );

		Task<User> AddUser( string userId, string username );

		Task<User> GetUser( string userId );

		Task<User> UpdateAvatarStatus( string userId, bool hasAvatar );
	}
}
