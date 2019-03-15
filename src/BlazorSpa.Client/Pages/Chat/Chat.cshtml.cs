using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorSpa.Client.Services;
using BlazorSpa.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace BlazorSpa.Client.Pages.Chat {
	public class ChatComponent : ComponentBase, IDisposable {

		public static string Url = "/chat";

		[Inject] private ILogger<ChatComponent> _logger { get; set; }
		[Inject] protected ISignalService SignalService { get; set; }

		internal string _toEverybody { get; set; }
		internal string _toConnection { get; set; }
		internal string _connectionId { get; set; }
		internal string _toMe { get; set; }
		internal string _toGroup { get; set; }
		internal string _groupName { get; set; }
		internal List<string> _messages { get; set; } = new List<string>();

		private IDisposable _sendHandle;
		private IDisposable _demoObjectHandle;

		protected override async Task OnInitAsync() {
			_sendHandle = SignalService.Register<string>( "Send", HandleTest );
			_demoObjectHandle = SignalService.Register<DemoData>( "DemoMethodObject", DemoMethodObject );
			await SignalService.Connect();
		}

		void IDisposable.Dispose() {
			_sendHandle.Dispose();
			_demoObjectHandle.Dispose();
		}

		public void HandleTest( string obj ) {
			Handle( obj );
		}

		public void DemoMethodObject( DemoData data ) {
			this._logger.LogInformation( "Got object!" );
			this._logger.LogInformation( data?.GetType().FullName ?? "<NULL>" );
			if( data == null ) return;
			this.Handle( data );
		}

		public void DemoMethodList( DemoData[] data ) {
			this._logger.LogInformation( "Got List!" );
			this._logger.LogInformation( data?.GetType().FullName ?? "<NULL>" );
			if( data == null ) return;
			this.Handle( data );
		}

		private void Handle( object msg ) {
			this._messages.Add( msg.ToString() );
			this.StateHasChanged();
		}

		internal async Task Broadcast() {
			await SignalService.Invoke( "Send", _toEverybody );
		}

		internal async Task SendToOthers() {
			await SignalService.Invoke( "SendToOthers", _toEverybody );
		}

		internal async Task SendToConnection() {
			await SignalService.Invoke( "SendToConnection", _connectionId, _toConnection );
		}

		internal async Task SendToMe() {
			await SignalService.Invoke( "Echo", _toMe );
		}

		internal async Task SendToGroup() {
			await SignalService.Invoke( "SendToGroup", _groupName, _toGroup );
		}

		internal async Task SendToOthersInGroup() {
			await SignalService.Invoke( "SendToOthersInGroup", _groupName, _toGroup );
		}

		internal async Task JoinGroup() {
			await SignalService.Invoke( "JoinGroup", _groupName );
		}

		internal async Task LeaveGroup() {
			await SignalService.Invoke( "LeaveGroup", _groupName );
		}

		internal async Task TellHubToDoStuff() {
			await SignalService.Invoke( "DoSomething", _groupName );
		}
	}
}