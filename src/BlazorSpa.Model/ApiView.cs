using System;
using Newtonsoft.Json;

namespace BlazorSpa.Model {
	public class ApiView {

		public ApiView() {
		}

		[JsonConstructor]
		public ApiView(
			string id,
			string viewType,
			string name
		) {
			Id = id;
			ViewType = viewType;
			Name = name;
		}

		public string Id { get; }

		public string ViewType { get; set; }

		public string Name { get; }
	}
}
