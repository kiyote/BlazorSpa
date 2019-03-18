using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorSpa.Shared {
	public static class ExtensionMethods {

		public static bool Similar<T>( this IEnumerable<T> source, IEnumerable<T> other ) {
			return ( source.Count() == other.Count() )
				&& ( source.Intersect( other ).Count() == source.Count() );
		}

		public static int GetFinalHashCode<T>( this IEnumerable<T> source ) {
			unchecked {
				var result = 17;
				foreach( var item in source ) {
					result = ( result * 31 ) + item.GetHashCode();
				}

				return result;
			}
		}
	}
}
