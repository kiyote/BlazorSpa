using System;
using BlazorSpa.Shared;
using Newtonsoft.Json;

namespace BlazorSpa.Client.Model {
	public class User {

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
	}
}
