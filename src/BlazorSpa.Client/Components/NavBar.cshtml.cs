using System.Diagnostics;
using System.Threading.Tasks;
using BlazorSpa.Client.Components;
using BlazorSpa.Client.Pages.Auth;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.JSInterop;

namespace BlazorSpa.Client.Layouts {
	public class NavBarComponent : BlazorComponent {
		private NavBarItemComponent _selectedItem;

		[Parameter] protected string Username { get; set; }

		[Parameter] protected bool IsAuthenticated { get; set; }

		[Inject] private IConfig _config { get; set; }

		protected NavBarItemComponent NavItemLogIn { get; set; }

		protected NavBarItemComponent NavItemProfile { get; set; }

		protected NavBarItemComponent NavItemChat { get; set; }

		protected NavBarItemComponent NavItemCounter { get; set; }

		protected NavBarItemComponent NavItemAdmin { get; set; }

		protected ElementRef Selector { get; set; }

		protected ElementRef TopBand { get; set; }

		public async Task SetActive( NavBarItemComponent item ) {
			if( item != _selectedItem ) {
				if( _selectedItem != default ) {
					_selectedItem.IsActive = false;
				}
				_selectedItem = item;
				if( _selectedItem != default ) {
					_selectedItem.IsActive = true;
				}
				await JSRuntime.Current.InvokeAsync<string>( "navBar.alignSelectorTo", TopBand, Selector, item?.ListItem );
				StateHasChanged();
			}
		}

		public string AuthUrl {
			get {
				return $"{_config.AuthUrl}&redirect_uri={_config.Host}{ValidateComponent.Url}";
			}
		}
	}
}