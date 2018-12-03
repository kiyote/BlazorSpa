using System;
using System.Threading.Tasks;
using BlazorSpa.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BlazorSpa.Server.Hubs {
	[Authorize( JwtBearerDefaults.AuthenticationScheme )]
	public class SignalHub : Hub {

		public readonly static string Url = "/signalhub";

		public async Task DoSomething( string groupName ) {
			await this.Clients.All.SendAsync( "DemoMethodObject", new DemoData { Id = 1, Data = "Demo Data" } );
			/*await this.Clients.All.SendAsync( "DemoMethodList",
				Enumerable.Range( 1, 10 ).Select( x => new DemoData { Id = x, Data = $"Demo Data #{x}" } ).ToList() );*/
		}

		public override async Task OnConnectedAsync() {
			var username = this.Context.GetHttpContext().Items[ "User" ];
			await this.Clients.Others.SendAsync( "Send", $"{username} joined" );
		}

		public override async Task OnDisconnectedAsync( Exception ex ) {
			var username = this.Context.GetHttpContext().Items[ "User" ];
			await this.Clients.Others.SendAsync( "Send", $"{username} left" );
		}

		public Task Send( string message ) {
			var username = this.Context.GetHttpContext().Items[ "User" ];
			return this.Clients.All.SendAsync( "Send", $"{username}: {message}" );
		}

		public Task SendToOthers( string message ) {
			var username = this.Context.GetHttpContext().Items[ "User" ];
			return this.Clients.Others.SendAsync( "Send", $"{username}: {message}" );
		}

		public Task SendToConnection( string connectionId, string message ) {
			return this.Clients.Client( connectionId )
				.SendAsync( "Send", $"Private message from {this.Context.ConnectionId}: {message}" );
		}

		public Task SendToGroup( string groupName, string message ) {
			return this.Clients.Group( groupName )
				.SendAsync( "Send", $"{this.Context.ConnectionId}@{groupName}: {message}" );
		}

		public Task SendToOthersInGroup( string groupName, string message ) {
			return this.Clients.OthersInGroup( groupName )
				.SendAsync( "Send", $"{this.Context.ConnectionId}@{groupName}: {message}" );
		}

		public async Task JoinGroup( string groupName ) {
			await this.Groups.AddToGroupAsync( this.Context.ConnectionId, groupName );

			await this.Clients.Group( groupName ).SendAsync( "Send", $"{this.Context.ConnectionId} joined {groupName}" );
		}

		public async Task LeaveGroup( string groupName ) {
			await this.Clients.Group( groupName ).SendAsync( "Send", $"{this.Context.ConnectionId} left {groupName}" );

			await this.Groups.RemoveFromGroupAsync( this.Context.ConnectionId, groupName );
		}

		public Task Echo( string message ) {
			return this.Clients.Caller.SendAsync( "Send", $"{this.Context.ConnectionId}: {message}" );
		}
	}
}
