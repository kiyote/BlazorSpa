
using System;
using Microsoft.AspNetCore.Blazor.Components;

namespace BlazorSpa.Client.Shared {
	public class NavMenuComponent : BlazorComponent, IDisposable {

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