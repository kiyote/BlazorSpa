using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Layouts;

namespace BlazorSpa.Client.Layouts {
	public class MainLayoutComponent : LayoutComponentBase, IDisposable {

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
