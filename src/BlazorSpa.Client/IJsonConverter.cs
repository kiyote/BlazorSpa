using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorSpa.Client {
	public interface IJsonConverter {
		T Deserialize<T>( string value );

		string Serialize( object value );
	}
}
