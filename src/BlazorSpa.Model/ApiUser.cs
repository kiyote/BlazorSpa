using System;
using Newtonsoft.Json;

namespace BlazorSpa.Model {
	public class ApiUser {

		[JsonConstructor]
		public ApiUser(
			string id,
			string name,
			string avatarUrl
		) {
			Id = id;
			Name = name;
			AvatarUrl = avatarUrl;
		}

		public string Id { get; }

		public string Name { get; }

		public string AvatarUrl { get; }
	}
}
