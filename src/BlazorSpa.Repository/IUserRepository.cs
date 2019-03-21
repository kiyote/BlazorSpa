﻿/*
 * Copyright 2018-2019 Todd Lang
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using System;
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
