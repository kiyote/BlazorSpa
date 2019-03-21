using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Layouts;

namespace BlazorSpa.Client.Pages {
	public class MainLayoutComponent : LayoutComponentBase, IDisposable {

		[Inject] private IAppState _state { get; set; }

		public string Username { get; set; }

		public bool IsAuthenticated { get; set; }

		protected override void OnInit() {
			_state.OnStateChanged += AppState_OnStateChanged;
		}

		protected override async Task OnInitAsync() {
			Username = await _state.GetUsername();
			IsAuthenticated = await _state.GetIsAuthenticated();
		}

		private void AppState_OnStateChanged( object sender, EventArgs e ) {
			StateHasChanged();
		}

		public void Dispose() {
			_state.OnStateChanged -= AppState_OnStateChanged;
		}
	}
}
