using System;
using Newtonsoft.Json;

namespace BlazorSpa.Shared {
	public class IdConverter : JsonConverter {
		public override bool CanConvert( Type objectType ) {
			return true;
		}

		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
			writer.WriteValue( value.ToString() );
		}

		public override bool CanRead {
			get { return true; }
		}

		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
			if( reader.Value == default ) {
				return Activator.CreateInstance( objectType, new object[] { Guid.Empty.ToString( "N" ) } );
			}

			return Activator.CreateInstance( objectType, new object[] { reader.Value as string } );
		}
	}
}
