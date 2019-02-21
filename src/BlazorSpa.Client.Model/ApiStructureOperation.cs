using Newtonsoft.Json;

namespace BlazorSpa.Client.Model {
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
