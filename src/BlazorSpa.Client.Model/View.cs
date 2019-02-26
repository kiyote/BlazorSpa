using System;
using BlazorSpa.Shared;
using Newtonsoft.Json;

namespace BlazorSpa.Client.Model {
	public class View: IEquatable<View> {

		[JsonConstructor]
		public View(
			Id<View> id,
			string viewType,
			string name
		) {
			Id = id;
			ViewType = viewType;
			Name = name;
		}

		public Id<View> Id { get; }

		public string ViewType { get; }

		public string Name { get; }

		public bool Equals( View other ) {
			if (ReferenceEquals(other, null)) {
				return false;
			}

			if (ReferenceEquals(other, this)) {
				return true;
			}

			return Id.Equals( other.Id )
				&& string.Equals( ViewType, other.ViewType, StringComparison.Ordinal )
				&& string.Equals( Name, other.Name, StringComparison.Ordinal );
		}

		public override bool Equals( object obj ) {
			return Equals( obj as View );
		}

		public override int GetHashCode() {
			unchecked {
				var result = 17;
				result = ( result * 31) + Id.GetHashCode();
				result = ( result * 31 ) + ViewType.GetHashCode();
				result = ( result * 31 ) + Name.GetHashCode();

				return result;
			}
		}
	}
}
