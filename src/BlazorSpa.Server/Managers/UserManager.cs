using System.Threading.Tasks;
using BlazorSpa.Repository;

namespace BlazorSpa.Server.Managers {
	public class UserManager {

		private readonly IUserRepository _userRepository;

		public UserManager(
			IUserRepository userRepository
		) {
			_userRepository = userRepository;
		}

		public async Task<UserInformationResult> GetUserInformation(string username) {
			return await _userRepository.GetUserInformation( username );
		}
	}
}
