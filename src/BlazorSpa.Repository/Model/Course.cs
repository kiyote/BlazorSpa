using System;
using System.Collections.Generic;
using System.Text;
using BlazorSpa.Shared;

namespace BlazorSpa.Repository.Model {
	public class Course: IEquatable<Course> {

		public Course( 
			Id<Course> id 
		) {
			Id = id;
		}

		public Id<Course> Id { get; }

		public bool Equals( Course other ) {
			if (ReferenceEquals( other, null)) {
				return false;
			}

			if (ReferenceEquals(other, this)) {
				return true;
			}

			return Id.Equals( other.Id );
		}

		public override bool Equals( object obj ) {
			return Equals( obj as Course );
		}

		public override int GetHashCode() {
			unchecked {
				var result = 31 * Id.GetHashCode();

				return result;
			}
		}
	}
}
