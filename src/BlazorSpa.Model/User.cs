using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorSpa.Model {
	public class User {

		public User(
			Id<User> userId,
			string authenticationId 
		) {
			Id = userId;
			AuthenticationId = authenticationId;
		}

		public Id<User> Id { get; }

		public string AuthenticationId { get; }
	}
}
