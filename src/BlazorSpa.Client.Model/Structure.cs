using System;
using Newtonsoft.Json;

namespace BlazorSpa.Client.Model {
	public class Structure {

		[JsonConstructor]
		public Structure(
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
