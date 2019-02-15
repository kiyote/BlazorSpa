using System;
using Newtonsoft.Json;

namespace BlazorSpa.Model {
	public class ApiView {

		[JsonConstructor]
		public ApiView(
			string id,
			string name
		) {
			Id = id;
			Name = name;
		}

		public string Id { get; }

		public string Name { get; }
	}
}
