using System;
using BlazorSpa.Shared;

namespace BlazorSpa.Repository.Model {
	public sealed class View: IEquatable<View> {

		public View(
			Id<View> viewId,
			string viewType,
			string name
		) {
			Id = viewId;
			ViewType = viewType;
			Name = name;
		}

		public Id<View> Id { get; }

		public string ViewType { get; }

		public string Name { get; }

		public override bool Equals( object obj ) {
			return Equals( obj as View );
		}

		public override int GetHashCode() {
			unchecked {
				var result = 31 * Id.GetHashCode();
				result = ( result * 31 ) + ViewType.GetHashCode();
				result = ( result * 31 ) + Name.GetHashCode();

				return result;
			}
		}

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
	}
}
