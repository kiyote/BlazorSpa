using System;
using BlazorSpa.Shared;

namespace BlazorSpa.Repository.Model {
	public sealed class Structure: IEquatable<Structure> {

		public Structure(
			Id<Structure> id,
			string structureType
		) {
			Id = id;
			StructureType = structureType;
		}

		public Id<Structure> Id { get; }

		public string StructureType { get; }

		public bool Equals( Structure other ) {
			if (ReferenceEquals(other, null)) {
				return false;
			}

			if (ReferenceEquals(other, this)) {
				return true;
			}

			return Id.Equals( other.Id )
				&& string.Equals( StructureType, other.StructureType, StringComparison.Ordinal );
		}

		public override bool Equals( object obj ) {
			return Equals( obj as Structure );
		}

		public override int GetHashCode() {
			unchecked {
				var result = 31 * Id.GetHashCode();
				result = ( result * 31 ) + StructureType.GetHashCode();

				return result;
			}
		}
	}
}
