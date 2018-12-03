using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorSpa.Client.Pages.Auth;
using Microsoft.AspNetCore.Blazor.Components;

namespace BlazorSpa.Client.Shared
{
    public class NavBarComponent : BlazorComponent, IDisposable
    {
		[Inject] protected AppState AppState { get; set; }

		[Inject] private IConfig _config { get; set; }

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

		public string AuthUrl {
			get {
				return $"{_config.AuthUrl}&redirect_uri={_config.Host}{ValidateComponent.Url}";
			}
		}

	}
}