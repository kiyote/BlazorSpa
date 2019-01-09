using System;
using Newtonsoft.Json;

namespace BlazorSpa.Model {
	public class ApiUser {

		[JsonConstructor]
		public ApiUser(
			string id,
			string name,
			string avatarUrl,
			string lastLogin
		) {
			Id = id;
			Name = name;
			AvatarUrl = avatarUrl;
			LastLogin = lastLogin;
		}

		public string Id { get; }

		public string Name { get; }

		public string AvatarUrl { get; }

		public string LastLogin { get; }
	}
}
