using System;
using System.Threading.Tasks;
using BlazorSpa.Model;

namespace BlazorSpa.Client.Services {
	public interface IUserApiService {
		Task<User> GetUserInformation();
	}
}
