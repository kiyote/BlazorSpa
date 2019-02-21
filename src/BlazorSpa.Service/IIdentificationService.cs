using System.Threading.Tasks;
using BlazorSpa.Repository.Model;
using BlazorSpa.Shared;

namespace BlazorSpa.Service {
	public interface IIdentificationService {
		Task<User> RecordLogin( string username );

		Task<User> GetUser( Id<User> userId );

		Task<User> SetAvatarStatus( Id<User> userId, bool hasAvatar );
	}
}
