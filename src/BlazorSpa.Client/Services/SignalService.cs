using System;
using System.Threading.Tasks;
using BlazorSignalR;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace BlazorSpa.Client.Services {
	internal sealed class SignalService : ISignalService {

		private readonly HubConnection _connection;

		public SignalService(
			IAccessTokenProvider accessTokenProvider,
			IJSRuntime jsRuntime
		) {
			HubConnectionBuilder factory = new HubConnectionBuilder();

			factory.WithUrlBlazor( "/signalhub", jsRuntime, options: opt => {
				opt.Transports = HttpTransportType.WebSockets | HttpTransportType.ServerSentEvents | HttpTransportType.LongPolling;
				opt.AccessTokenProvider = accessTokenProvider.GetJwtToken;
			} );

			_connection = factory.Build();

			_connection.Closed += exception => {
				_connection.StartAsync();
				return Task.CompletedTask;
			};
		}

		IDisposable ISignalService.Register<T>( string name, Action<T> handler ) {
			return _connection.On( name, handler );
		}

		async Task ISignalService.Connect() {
			await _connection.StartAsync();
		}

		async Task ISignalService.Invoke<T>( string name, T payload ) {
			await _connection.InvokeAsync( name, payload );
		}

		async Task ISignalService.Invoke<S, T>( string name, S arg1, T arg2 ) {
			await _connection.InvokeAsync( name, arg1, arg2 );
		}
	}
}
