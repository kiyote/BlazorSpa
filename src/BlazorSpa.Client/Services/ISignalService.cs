using System;
using System.Threading.Tasks;

namespace BlazorSpa.Client.Services {
	public interface ISignalService {

		Task Connect();

		IDisposable Register<T>( string name, Action<T> callback );

		Task Invoke<T>( string name, T payload );

		Task Invoke<S, T>( string name, S arg1, T arg2 );
	}
}
