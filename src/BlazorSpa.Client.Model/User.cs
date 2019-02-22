using System;
using BlazorSpa.Shared;
using Newtonsoft.Json;

namespace BlazorSpa.Client.Model {
	public class User: IEquatable<User> {

		[JsonConstructor]
		public User(
			Id<User> id,
			string name,
			string avatarUrl,
			DateTime lastLogin,
			DateTime? previousLogin
		) {
			Id = id;
			Name = name;
			AvatarUrl = avatarUrl;
			LastLogin = lastLogin;
			PreviousLogin = previousLogin;
		}

		public Id<User> Id { get; }

		public string Name { get; }

		public string AvatarUrl { get; }

		public DateTime LastLogin { get; }

		public DateTime? PreviousLogin { get; }

		public bool Equals( User other ) {
			if (ReferenceEquals(other, null)) {
				return false;
			}

			if (ReferenceEquals(other, this)) {
				return true;
			}

			return Id.Equals( other.Id )
				&& string.Equals( Name, other.Name, StringComparison.Ordinal )
				&& string.Equals( AvatarUrl, other.AvatarUrl, StringComparison.Ordinal )
				&& DateTime.Equals( LastLogin, other.LastLogin )
				&& Nullable.Equals( PreviousLogin, other.PreviousLogin );
		}

		public override bool Equals( object obj ) {
			return Equals( obj as User );
		}

		public override int GetHashCode() {
			unchecked {
				var result = 17;
				result = ( result * 31 ) * Id.GetHashCode();
				result = ( result * 31 ) + Name.GetHashCode();
				result = ( result * 31 ) + AvatarUrl.GetHashCode();
				result = ( result * 31 ) + LastLogin.GetHashCode();
				result = ( result * 31 ) + PreviousLogin?.GetHashCode() ?? 0;

				return result;
			}
		}
	}
}
