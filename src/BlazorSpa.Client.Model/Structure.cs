using Newtonsoft.Json;

namespace BlazorSpa.Client.Model {
	public class Structure {

		[JsonConstructor]
		public Structure(
			string id,
			string structureType,
			string name
		) {
			Id = id;
			StructureType = structureType;
			Name = name;
		}

		public string Id { get; }

		public string StructureType { get; }

		public string Name { get; }
	}
}
