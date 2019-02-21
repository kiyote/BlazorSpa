﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorSpa.Repository.Model;
using BlazorSpa.Shared;

namespace BlazorSpa.Repository {
	public interface IUserRepository {

		Task<User> GetByUsername( string username );

		Task<User> AddUser( Id<User> userId, string username, DateTime createdOn, DateTime lastLogin );

		Task<User> GetUser( Id<User> userId );

		Task<User> UpdateAvatarStatus( Id<User> userId, bool hasAvatar );

		Task<User> SetLastLogin( Id<User> userId, DateTime lastLogin );

		Task<IEnumerable<Id<View>>> GetViewIds( Id<User> userId );

		Task AddView( Id<User> userId, Id<View> viewId, DateTime dateCreated );

		Task RemoveView( Id<User> userId, Id<View> viewId );
	}
}
