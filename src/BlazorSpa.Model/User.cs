using System;
using Newtonsoft.Json;

namespace BlazorSpa.Model {
	public class User {

		public User(
			Id<User> userId,
			string name,
			string authenticationId,
			DateTimeOffset lastLogin
		) {
			Id = userId;
			Name = name;
			AuthenticationId = authenticationId;
			LastLogin = lastLogin;
		}

		[JsonConstructor]
		public User(
			string userId,
			string name,
			string authenticationId,
			string lastLogin ) :
			this( new Id<User>( userId ), name, authenticationId, DateTimeOffset.Parse( lastLogin ) 
		) {
		}

		public Id<User> Id { get; }

		public string Name { get; }

		public string AuthenticationId { get; }

		public DateTimeOffset LastLogin { get; }
	}
}
