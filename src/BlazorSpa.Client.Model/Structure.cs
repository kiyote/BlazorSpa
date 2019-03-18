using System;
using BlazorSpa.Shared;
using Newtonsoft.Json;

namespace BlazorSpa.Client.Model {
	public class Structure : IEquatable<Structure> {

		[JsonConstructor]
		public Structure(
			Id<Structure> id,
			string structureType,
			string name
		) {
			Id = id;
			StructureType = structureType;
			Name = name;
		}

		public Id<Structure> Id { get; }

		public string StructureType { get; }

		public string Name { get; }

		public bool Equals( Structure other ) {
			if( ReferenceEquals( other, null ) ) {
				return false;
			}

			if( ReferenceEquals( other, this ) ) {
				return true;
			}

			return Id.Equals( other.Id )
				&& string.Equals( StructureType, other.StructureType, StringComparison.Ordinal )
				&& string.Equals( Name, other.Name, StringComparison.Ordinal );
		}

		public override bool Equals( object obj ) {
			return Equals( obj as Structure );
		}

		public override int GetHashCode() {
			unchecked {
				var result = 17;
				result = ( result * 31 ) + Id.GetHashCode();
				result = ( result * 31 ) + StructureType.GetHashCode();
				result = ( result * 31 ) + Name.GetHashCode();

				return result;
			}
		}
	}
}
