using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
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
			var authentication = new AuthenticationRecord {
				AuthenticationId = authenticationId,
				Status = UserRecord.Active
			};
			authentication = await _context.LoadAsync( authentication );

			if (authentication == default) {
				return default;
			}

			var search = _context.QueryAsync<UserRecord>( 
				authentication.UserId, 
				QueryOperator.BeginsWith, 
				new List<object>() { $"{UserRecord.Active}/" } 
			);

			var userRecords = await search.GetRemainingAsync();
			var userRecord = userRecords.FirstOrDefault();

			if (userRecord == default) {
				return default;
			}

			return new User(
				new Id<User>( userRecord.UserId ),
				userRecord.Name,
				userRecord.AuthenticationId,
				new DateTimeOffset( userRecord.LastLogin ) 
			);
		}

		async Task<User> IUserRepository.AddUser( Id<User> userId, string authenticationId, string username ) {
			var authentication = new AuthenticationRecord {
				AuthenticationId = authenticationId,
				Status = UserRecord.Active,
				UserId = userId.Value
			};
			await _context.SaveAsync( authentication );

			var lastLogin = DateTimeOffset.Now;
			var user = new UserRecord {
				UserId = authentication.UserId,
				Status = UserRecord.Active,
				Name = username,
				LastLogin = lastLogin.UtcDateTime
			};
			await _context.SaveAsync( user );

			return new User( 
				userId, 
				username,
				authenticationId, 
				lastLogin );
		}

		async Task<User> IUserRepository.GetUser( Id<User> userId ) {
			var search = _context.QueryAsync<UserRecord>(
				userId.Value,
				QueryOperator.BeginsWith,
				new List<object>() { $"{UserRecord.Active}/" }
			);

			var userRecords = await search.GetRemainingAsync();
			var userRecord = userRecords.FirstOrDefault();

			return new User(
				new Id<User>( userRecord.UserId ),
				userRecord.Name,
				userRecord.AuthenticationId,
				new DateTimeOffset(userRecord.LastLogin)
			);
		}
	}
}
