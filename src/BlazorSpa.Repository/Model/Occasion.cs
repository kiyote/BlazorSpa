using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlazorSpa.Shared;

namespace BlazorSpa.Repository.Model {
	public sealed class Occasion : IEquatable<Occasion> {

		public Occasion(
			Id<Occasion> id,
			IEnumerable<DateTime> occursOn,
			string name
		) {
			Id = id;
			OccursOn = occursOn.Select(o => o.ToUniversalTime());
			Name = name;
		}

		public Id<Occasion> Id { get; }

		public IEnumerable<DateTime> OccursOn { get; }

		public string Name { get; }

		public bool Equals( Occasion other ) {
			if (ReferenceEquals(other, null)) {
				return false;
			}

			if (ReferenceEquals(other, this)) {
				return true;
			}

			return Id.Equals( other.Id )
				&& OccursOn.Similar( other.OccursOn );
		}

		public override bool Equals( object obj ) {
			return Equals( obj as Occasion );
		}

		public override int GetHashCode() {
			unchecked {
				var result = 17;
				result = ( result * 31 ) + Id.GetHashCode();
				result = ( result * 31 ) + OccursOn.GetFinalHashCode();

				return result;
			}
		}
	}
}
