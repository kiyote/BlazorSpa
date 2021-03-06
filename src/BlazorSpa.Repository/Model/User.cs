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
using BlazorSpa.Shared;

namespace BlazorSpa.Repository.Model {
	public sealed class User : IEquatable<User> {

		public User(
			Id<User> id,
			string name,
			bool hasAvatar,
			DateTime lastLogin,
			DateTime? previousLogin
		) {
			Id = id;
			Name = name;
			HasAvatar = hasAvatar;
			LastLogin = lastLogin.ToUniversalTime();
			PreviousLogin = previousLogin?.ToUniversalTime();
		}

		public Id<User> Id { get; }

		public string Name { get; }

		public bool HasAvatar { get; }

		public DateTime LastLogin { get; }

		public DateTime? PreviousLogin { get; }

		public static string CreateId() {
			return Guid.NewGuid().ToString( "N" );
		}

		public bool Equals( User other ) {
			if( ReferenceEquals( other, this ) ) {
				return true;
			}

			if( ReferenceEquals( other, null ) ) {
				return false;
			}

			return Id.Equals( other.Id )
				&& string.Equals( Name, other.Name, StringComparison.Ordinal )
				&& ( HasAvatar == other.HasAvatar )
				&& ( LastLogin.Equals( other.LastLogin ) )
				&& ( PreviousLogin.Equals( other.PreviousLogin ) );
		}

		public override bool Equals( object obj ) {
			return Equals( obj as User );
		}

		public override int GetHashCode() {
			unchecked {
				var result = 17;
				result = ( result * 31 ) + Id.GetHashCode();
				result = ( result * 31 ) + Name.GetHashCode();
				result = ( result * 31 ) + HasAvatar.GetHashCode();
				result = ( result * 31 ) + LastLogin.GetHashCode();
				result = ( result * 31 ) + PreviousLogin.GetHashCode();

				return result;
			}
		}
	}
}
