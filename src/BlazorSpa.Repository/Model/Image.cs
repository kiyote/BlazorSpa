using System;

namespace BlazorSpa.Repository.Model {
	public sealed class Image: IEquatable<Image> {

		public Image(
			Id<Image> imageId,
			string url
		) {
			Id = imageId;
			Url = url;
		}

		public Id<Image> Id { get; }

		public string Url { get; }

		bool IEquatable<Image>.Equals( Image other ) {
			if (ReferenceEquals(other, default)) {
				return false;
			}

			if (ReferenceEquals(other, this)) {
				return true;
			}

			return Id.Equals( other.Id )
				&& string.Equals( Url, other.Url, StringComparison.OrdinalIgnoreCase );
		}

		public override bool Equals( object obj ) {
			return Equals( obj as Image );
		}

		public override int GetHashCode() {
			unchecked {
				var result = 31 * Id.GetHashCode();
				result = ( result * 31 ) + StringComparer.OrdinalIgnoreCase.GetHashCode( Url );

				return result;
			}
		}
	}
}
