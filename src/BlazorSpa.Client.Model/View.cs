using BlazorSpa.Shared;
using Newtonsoft.Json;

namespace BlazorSpa.Client.Model {
	public class View {

		[JsonConstructor]
		public View(
			string id,
			string viewType,
			string name
		) {
			Id = new Id<View>(id);
			ViewType = viewType;
			Name = name;
		}

		public Id<View> Id { get; }

		public string ViewType { get; }

		public string Name { get; }
	}
}
