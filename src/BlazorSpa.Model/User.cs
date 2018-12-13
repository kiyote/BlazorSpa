using System;
using Newtonsoft.Json;

namespace BlazorSpa.Model {
	public class User {

		[JsonConstructor]
		public User(
			string id,
			string name,
			string authenticationId
		) {
			Id = id;
			Name = name;
			AuthenticationId = authenticationId;
		}

		public string Id { get; }

		public string Name { get; }

		public string AuthenticationId { get; }

		public static string CreateId() {
			return Guid.NewGuid().ToString( "N" );
		}
	}
}
