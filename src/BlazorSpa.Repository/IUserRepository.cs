using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlazorSpa.Model;

namespace BlazorSpa.Repository {
	public interface IUserRepository {

		Task<User> GetByAuthenticationId( string authenticationId );

		Task<User> AddUser( string userId, string authenticationId, string username );

		Task<User> GetUser( string userId );
	}
}
