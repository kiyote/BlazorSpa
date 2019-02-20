using System;
using System.Threading.Tasks;
using BlazorSpa.Client.Model;

namespace BlazorSpa.Client.Services {
	public interface IUserApiService {
		Task<User> GetUserInformation();

		Task<string> SetAvatar( string contentType, string content );

		Task RecordLogin();
	}
}
