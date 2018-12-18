using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using BlazorSpa.Repository.Model;
using BlazorSpa.Repository.DynamoDb.Model;

namespace BlazorSpa.Repository.DynamoDb {
	public class UserRepository : IUserRepository {

		private readonly IDynamoDBContext _context;

		public UserRepository(
			IAmazonDynamoDB client
		) {
			_context = new DynamoDBContext( client );
		}

		async Task<User> IUserRepository.GetByUsername( string username ) {
			var authentication = new AuthenticationRecord {
				Username = username,
				Status = UserRecord.Active
			};
			authentication = await _context.LoadAsync( authentication );

			if (authentication == default) {
				return default;
			}

			var search = _context.QueryAsync<UserRecord>( 
				authentication.UserId, 
				QueryOperator.Equal, 
				new List<object>() { UserRecord.Active } 
			);

			var userRecords = await search.GetRemainingAsync();
			var userRecord = userRecords.FirstOrDefault();

			if (userRecord == default) {
				return default;
			}

			return new User(
				userRecord.UserId,
				userRecord.Name,
				userRecord.HasAvatar);
		}

		async Task<User> IUserRepository.AddUser( string userId, string username ) {
			var authentication = new AuthenticationRecord {
				Username = username,
				Status = UserRecord.Active,
				UserId = userId
			};
			await _context.SaveAsync( authentication );

			var user = new UserRecord {
				UserId = authentication.UserId,
				Status = UserRecord.Active,
				Name = username
			};
			await _context.SaveAsync( user );

			return new User( 
				userId, 
				username,
				false);
		}

		async Task<User> IUserRepository.GetUser( string userId ) {
			var userRecord = await GetById( userId );

			return new User(
				userRecord.UserId,
				userRecord.Name,
				userRecord.HasAvatar);
		}

		async Task<User> IUserRepository.UpdateAvatarStatus(string userId, bool hasAvatar) {
			var userRecord = await GetById( userId );
			userRecord.HasAvatar = true;
			await _context.SaveAsync( userRecord );

			return new User(
				userRecord.UserId,
				userRecord.Name,
				userRecord.HasAvatar );
		}

		private async Task<UserRecord> GetById( string userId ) {
			var search = _context.QueryAsync<UserRecord>(
				userId,
				QueryOperator.Equal,
				new List<object>() { UserRecord.Active }
			);

			var userRecords = await search.GetRemainingAsync();
			var userRecord = userRecords.FirstOrDefault();

			return userRecord;
		}
	}
}
