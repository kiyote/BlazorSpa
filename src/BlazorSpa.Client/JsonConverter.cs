using Newtonsoft.Json;

namespace BlazorSpa.Client {
	public class JsonConverter : IJsonConverter {

		private readonly JsonSerializerSettings _settings;

		public JsonConverter() {
			_settings = new JsonSerializerSettings() {
				DateParseHandling = DateParseHandling.None,
				DateTimeZoneHandling = DateTimeZoneHandling.Utc,
				DateFormatHandling = DateFormatHandling.IsoDateFormat
			};
		}

		T IJsonConverter.Deserialize<T>( string value ) {
			return JsonConvert.DeserializeObject<T>( value, _settings );
		}

		string IJsonConverter.Serialize( object value ) {
			if( value == default ) {
				return "{}";
			}
			return JsonConvert.SerializeObject( value, _settings );
		}
	}
}
