using System;

namespace BlazorSpa.Repository.Model {
	public class User : IEquatable<User> {

		public User(
			Id<User> id,
			string name,
			bool hasAvatar,
			DateTimeOffset lastLogin
		) {
			Id = id;
			Name = name;
			HasAvatar = hasAvatar;
			LastLogin = lastLogin;
		}

		public Id<User> Id { get; }

		public string Name { get; }

		public bool HasAvatar { get; }

		public DateTimeOffset LastLogin { get; }

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
				&& ( LastLogin.Equals( other.LastLogin ) );
		}

		public override bool Equals( object obj ) {
			return Equals( obj as User );
		}

		public override int GetHashCode() {
			unchecked {
				var result = 31 * Id.GetHashCode();
				result = ( result * 31 ) + Name.GetHashCode();
				result = ( result * 31 ) + HasAvatar.GetHashCode();
				result = ( result * 31 ) + LastLogin.GetHashCode();

				return result;
			}
		}
	}
}