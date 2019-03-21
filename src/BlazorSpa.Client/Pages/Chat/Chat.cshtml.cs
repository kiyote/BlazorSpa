/*
 * Copyright 2018-2019 Todd Lang
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
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
		[Inject] private ISignalService _signalService { get; set; }

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
			_sendHandle = _signalService.Register<string>( "Send", HandleTest );
			_demoObjectHandle = _signalService.Register<DemoData>( "DemoMethodObject", DemoMethodObject );
			await _signalService.Connect();
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
			await _signalService.Invoke( "Send", _toEverybody );
		}

		internal async Task SendToOthers() {
			await _signalService.Invoke( "SendToOthers", _toEverybody );
		}

		internal async Task SendToConnection() {
			await _signalService.Invoke( "SendToConnection", _connectionId, _toConnection );
		}

		internal async Task SendToMe() {
			await _signalService.Invoke( "Echo", _toMe );
		}

		internal async Task SendToGroup() {
			await _signalService.Invoke( "SendToGroup", _groupName, _toGroup );
		}

		internal async Task SendToOthersInGroup() {
			await _signalService.Invoke( "SendToOthersInGroup", _groupName, _toGroup );
		}

		internal async Task JoinGroup() {
			await _signalService.Invoke( "JoinGroup", _groupName );
		}

		internal async Task LeaveGroup() {
			await _signalService.Invoke( "LeaveGroup", _groupName );
		}

		internal async Task TellHubToDoStuff() {
			await _signalService.Invoke( "DoSomething", _groupName );
		}
	}
}
