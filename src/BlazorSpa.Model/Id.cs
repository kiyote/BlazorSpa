using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BlazorSpa.Shared {
	[JsonConverter( typeof( IdConverter ) )]
	public sealed class Id<T> : IEquatable<Id<T>> {

		public static readonly Id<T> Empty = new Id<T>( Guid.Empty.ToString("N"), false );

		public static readonly IEnumerable<Id<T>> EmptyList = new List<Id<T>>();

		public Id() {
			Value = Guid.NewGuid().ToString( "N" );
		}

		public Id( Guid id ) {
			if( id == Guid.Empty ) {
				throw new ArgumentException( nameof( id ) );
			}

			Value = id.ToString( "N" );
		}

		public Id( string value ) :
			this( value, false ) {
		}

		private Id( string value, bool validate ) {
			if( !validate ) {
				Value = value;
			} else {
				if( string.IsNullOrWhiteSpace( value ) ) {
					throw new ArgumentException( value );
				}

				if( !Guid.TryParse( value, out Guid id ) ) {
					throw new ArgumentException( nameof( value ) );
				}

				if( id == Guid.Empty ) {
					throw new ArgumentException( nameof( value ) );
				}

				Value = id.ToString( "N" );
			}

		}

		public string Value { get; }

		public override string ToString() {
			return Value;
		}

		public override int GetHashCode() {
			return StringComparer.Ordinal.GetHashCode( Value );
		}

		public override bool Equals( object obj ) {
			return Equals( obj as Id<T> );
		}

		public bool Equals( Id<T> other ) {
			if( ReferenceEquals( other, default( Id<T> ) ) ) {
				return false;
			}

			if( ReferenceEquals( other, this ) ) {
				return true;
			}

			return
				string.CompareOrdinal( Value, other.Value ) == 0;
		}

		public static bool IsValid( string value ) {
			if( string.IsNullOrWhiteSpace( value ) ) {
				return false;
			}

			if( !Guid.TryParse( value, out Guid id ) ) {
				return false;
			}

			if( id == Guid.Empty ) {
				return false;
			}

			return true;
		}

		public static bool operator ==( Id<T> left, Id<T> right ) {
			if( ReferenceEquals( left, right ) ) {
				return true;
			}

			// If one is null, but not both, return false.
			if( ReferenceEquals( left, default( Id<T> ) ) || ReferenceEquals( right, default( Id<T> ) ) ) {
				return false;
			}

			return
				string.CompareOrdinal( left?.Value, right?.Value ) == 0;
		}

		public static bool operator !=( Id<T> left, Id<T> right ) {
			return !( left == right );
		}
	}
}