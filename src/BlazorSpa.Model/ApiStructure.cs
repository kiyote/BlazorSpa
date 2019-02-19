using System;
using Newtonsoft.Json;

namespace BlazorSpa.Model {
	public class ApiStructure {

		[JsonConstructor]
		public ApiStructure(
			string id,
			string structureType
		) {
			Id = id;
			StructureType = structureType;
		}

		public string Id { get; }

		public string StructureType { get; }
	}
}
