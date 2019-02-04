using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Layouts;
using Microsoft.JSInterop;

namespace BlazorSpa.Client.Layouts {
	public class MainLayoutComponent : BlazorLayoutComponent, IDisposable {

		[Inject] protected AppState AppState { get; set; }

		protected override void OnInit() {
			base.OnInit();
			AppState.OnStateChanged += AppState_OnStateChanged;
		}

		private void AppState_OnStateChanged( object sender, EventArgs e ) {
			StateHasChanged();
		}

		public void Dispose() {
			AppState.OnStateChanged -= AppState_OnStateChanged;
		}
	}
}
