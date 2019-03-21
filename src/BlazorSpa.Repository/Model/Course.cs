/*
 * Copyright 2018-2019 Todd Lang
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using System;
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
