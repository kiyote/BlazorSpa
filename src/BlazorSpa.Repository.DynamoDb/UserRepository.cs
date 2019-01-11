using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using BlazorSpa.Repository.Model;
using BlazorSpa.Repository.DynamoDb.Model;

namespace BlazorSpa.Repository.DynamoDb {
	public class UserRepository : IUserRepository {

		private readonly IDynamoDBContext _context;

		public UserRepository(
			IDynamoDBContext context
		) {
			_context = context;
		}

		async Task<User> IUserRepository.GetByUsername(
			string username
		) {
			var authentication = new AuthenticationRecord {
				Identifier = username
			};
			authentication = await _context.LoadAsync( authentication );

			if( !( authentication?.Status == AuthenticationRecord.StatusActive ) ) {
				return default;
			}

			var search = _context.QueryAsync<UserRecord>(
				authentication.UserId,
				QueryOperator.Equal,
				new List<object>() { UserRecord.UserItemType }
			);

			var userRecords = await search.GetRemainingAsync();
			var userRecord = userRecords.FirstOrDefault();

			if( userRecord == default ) {
				return default;
			}

			return new User(
				new Id<User>( userRecord.UserId ),
				userRecord.Name,
				userRecord.HasAvatar,
				new DateTimeOffset( userRecord.LastLogin ),
				userRecord.PreviousLogin.HasValue
					? new DateTimeOffset( userRecord.PreviousLogin.Value ) 
					: default );
		}

		async Task<User> IUserRepository.AddUser(
			Id<User> userId,
			string username,
			DateTimeOffset lastLogin
		) {
			var authentication = new AuthenticationRecord {
				Identifier = username,
				UserId = userId.Value,
				DateCreated = DateTime.UtcNow,
				Status = AuthenticationRecord.StatusActive
			};
			await _context.SaveAsync( authentication );

			var user = new UserRecord {
				UserId = authentication.UserId,
				Name = username,
				HasAvatar = false,
				LastLogin = lastLogin.UtcDateTime,
				PreviousLogin = default,
				Status = UserRecord.Active
			};
			await _context.SaveAsync( user );

			return new User(
				userId,
				username,
				false,
				lastLogin,
				default );
		}

		async Task<User> IUserRepository.GetUser(
			Id<User> userId
		) {
			var userRecord = await GetById( userId );
			return ToUser( userId, userRecord );
		}

		async Task<User> IUserRepository.UpdateAvatarStatus(
			Id<User> userId,
			bool hasAvatar
		) {
			var userRecord = await GetById( userId );
			userRecord.HasAvatar = true;
			await _context.SaveAsync( userRecord );

			return ToUser( userId, userRecord );
		}

		async Task<User> IUserRepository.SetLastLogin(
			Id<User> userId,
			DateTimeOffset lastLogin
		) {
			var userRecord = await GetById( userId );
			userRecord.PreviousLogin = userRecord.LastLogin;
			userRecord.LastLogin = lastLogin.UtcDateTime;
			await _context.SaveAsync( userRecord );

			return ToUser( userId, userRecord );
		}

		private async Task<UserRecord> GetById(
			Id<User> userId
		) {
			var search = _context.QueryAsync<UserRecord>(
				userId.Value,
				QueryOperator.Equal,
				new List<object>() { UserRecord.UserItemType }
			);

			var userRecords = await search.GetRemainingAsync();
			var userRecord = userRecords.FirstOrDefault();

			return userRecord;
		}

		private static User ToUser(Id<User> userId, UserRecord userRecord) {
			return new User(
				userId,
				userRecord.Name,
				userRecord.HasAvatar,
				new DateTimeOffset( userRecord.LastLogin ),
				userRecord.PreviousLogin.HasValue
					? new DateTimeOffset( userRecord.PreviousLogin.Value )
					: default( DateTimeOffset? ) );
		}
	}
}