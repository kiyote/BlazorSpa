using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using BlazorSpa.Repository.DynamoDb.Model;
using BlazorSpa.Repository.Model;
using BlazorSpa.Shared;

namespace BlazorSpa.Repository.DynamoDb {
	public sealed class UserRepository : IUserRepository {

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
				Username = username
			};
			authentication = await _context.LoadAsync( authentication );

			if( !( authentication?.Status == AuthenticationRecord.StatusActive ) ) {
				return default;
			}

			var userKey = UserRecord.GetKey( authentication.UserId );
			var search = _context.QueryAsync<UserRecord>(
				userKey,
				QueryOperator.Equal,
				new List<object>() { userKey }
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
				userRecord.LastLogin,
				userRecord.PreviousLogin );
		}
		

		async Task<User> IUserRepository.AddUser(
			Id<User> userId,
			string username,
			DateTime createdOn,
			DateTime lastLogin
		) {
			var authentication = new AuthenticationRecord {
				Username = username,
				UserId = userId.Value,
				DateCreated = createdOn.ToUniversalTime(),
				Status = AuthenticationRecord.StatusActive
			};
			await _context.SaveAsync( authentication );

			var user = new UserRecord {
				UserId = authentication.UserId,
				Name = username,
				HasAvatar = false,
				LastLogin = lastLogin.ToUniversalTime(),
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
			DateTime lastLogin
		) {
			var userRecord = await GetById( userId );
			userRecord.PreviousLogin = userRecord.LastLogin;
			userRecord.LastLogin = lastLogin.ToUniversalTime();
			await _context.SaveAsync( userRecord );

			return ToUser( userId, userRecord );
		}

		async Task<IEnumerable<Id<View>>> IUserRepository.GetViewIds( Id<User> userId ) {
			var query = _context.QueryAsync<UserViewRecord>(
				UserRecord.GetKey( userId.Value ),
				QueryOperator.BeginsWith,
				new List<object>() { ViewRecord.ViewItemType } );

			var records = await query.GetRemainingAsync();

			var result = new List<Id<View>>();
			foreach( var record in records ) {
				result.Add( new Id<View>( record.ViewId ) );
			}

			return result;
		}

		async Task IUserRepository.AddView( 
			Id<User> userId, 
			Id<View> viewId, 
			DateTime createdOn
		) {
			var userViewRecord = new UserViewRecord() {
				DateCreated = createdOn.ToUniversalTime(),
				UserId = userId.Value,
				ViewId = viewId.Value
			};

			await _context.SaveAsync( userViewRecord );
		}

		async Task IUserRepository.RemoveView( Id<User> userId, Id<View> viewId ) {
			await _context.DeleteAsync<UserViewRecord>( UserRecord.GetKey( userId.Value ), ViewRecord.GetKey( viewId.Value ));
		}

		private async Task<UserRecord> GetById(
			Id<User> userId
		) {
			var userKey = UserRecord.GetKey( userId.Value );
			var search = _context.QueryAsync<UserRecord>(
				userKey,
				QueryOperator.Equal,
				new List<object>() { userKey }
			);

			var userRecords = await search.GetRemainingAsync();
			var userRecord = userRecords.FirstOrDefault();

			return userRecord;
		}

		private static User ToUser( Id<User> userId, UserRecord userRecord ) {
			return new User(
				userId,
				userRecord.Name,
				userRecord.HasAvatar,
				userRecord.LastLogin,
				userRecord.PreviousLogin
			);
		}
	}
}