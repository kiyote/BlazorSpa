using System;

namespace BlazorSpa.Repository.Model {
	public class User {

		public User(
			Id<User> id,
			string name,
			bool hasAvatar
		) {
			Id = id;
			Name = name;
			HasAvatar = hasAvatar;
		}

		public Id<User> Id { get; }

		public string Name { get; }

		public bool HasAvatar { get; }

		public static string CreateId() {
			return Guid.NewGuid().ToString( "N" );
		}
	}
}
