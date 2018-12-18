﻿using System;
using Newtonsoft.Json;

namespace BlazorSpa.Model {
	public class User {

		[JsonConstructor]
		public User(
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

		public static string CreateId() {
			return Guid.NewGuid().ToString( "N" );
		}
	}
}
