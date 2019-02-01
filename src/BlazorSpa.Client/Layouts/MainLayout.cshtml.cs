using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Layouts;
using Microsoft.JSInterop;

namespace BlazorSpa.Client.Layouts {
	public class MainLayoutComponent : BlazorLayoutComponent, IDisposable {

		protected bool UpdateReady;

		[Inject] protected AppState AppState { get; set; }

		protected override async Task OnInitAsync() {
			await JSRuntime
			  .Current
			  .InvokeAsync<object>(
				"blazorFuncs.registerClient",
				new DotNetObjectRef( this )
			  );
		}

		protected override void OnInit() {
			base.OnInit();
			AppState.OnStateChanged += AppState_OnStateChanged;
		}

		private void AppState_OnStateChanged( object sender, EventArgs e ) {
			StateHasChanged();
		}

		public void Dispose() {
			AppState.OnStateChanged += AppState_OnStateChanged;
		}

		[JSInvokable( "onupdateavailable" )]
		public async Task<string> AppUpdate() {
			Console.WriteLine( "New version available" );
			UpdateReady = true;
			StateHasChanged();
			return await Task.FromResult( "Alerted client" );
		}
	}
}
