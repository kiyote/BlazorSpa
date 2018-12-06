using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using BlazorSpa.Model;
using BlazorSpa.Repository.DynamoDb.Model;

namespace BlazorSpa.Repository.DynamoDb {
	public class UserRepository : IUserRepository {

		private readonly IDynamoDBContext _context;

		public UserRepository(
			IAmazonDynamoDB client
		) {
			_context = new DynamoDBContext( client );
		}

		async Task<User> IUserRepository.GetByAuthenticationId( string authenticationId ) {
			var results = _context.QueryAsync<DynamoUser>(
				authenticationId,
				new DynamoDBOperationConfig {
					IndexName = "AuthenticationId-index"
				} );
			var result = await results.GetRemainingAsync();

			return result.FirstOrDefault()?.ToUser();
		}

		async Task<User> IUserRepository.AddUser( Id<User> userId, string authenticationId ) {
			var user = new User( userId, authenticationId );
			var dynamoUser = new DynamoUser( user );
			await _context.SaveAsync( dynamoUser );

			return user;
		}

		async Task<User> IUserRepository.GetUser( Id<User> userId ) {
			var dynamoUser = new DynamoUser() { UserId = userId.Value };
			return ( await _context.LoadAsync( dynamoUser ) )?.ToUser();
		}
	}
}
