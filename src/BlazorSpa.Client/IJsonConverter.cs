namespace BlazorSpa.Client {
	public interface IJsonConverter {
		T Deserialize<T>( string value );

		string Serialize( object value );
	}
}
