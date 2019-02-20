using System;
using Newtonsoft.Json;

namespace BlazorSpa.Model {
	public class ApiStructureOperation {

		[JsonConstructor]
		public ApiStructureOperation(
			string status
		) {
			Status = status;
		}

		public string Status { get; }
	}
}
