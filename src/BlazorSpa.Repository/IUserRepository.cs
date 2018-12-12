using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlazorSpa.Model;

namespace BlazorSpa.Repository {
	public interface IUserRepository {

		Task<User> GetByAuthenticationId( string authenticationId );

		Task<User> AddUser( Id<User> userId, string authenticationId, string username );

		Task<User> GetUser( Id<User> userId );
	}
}
