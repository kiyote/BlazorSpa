using System;
using System.Threading.Tasks;
using BlazorSpa.Model.Api;

namespace BlazorSpa.Client.Services {
	public interface IUserApiService {
		Task<UserInformationResponse> GetUserInformation( UserInformationRequest request );
	}
}
