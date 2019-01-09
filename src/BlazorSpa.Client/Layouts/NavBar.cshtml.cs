using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BlazorSpa.Client.Components;
using BlazorSpa.Client.Pages.Auth;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;

namespace BlazorSpa.Client.Layouts {
    public class NavBarComponent : BlazorComponent
    {
		private NavBarItemComponent _selectedItem;

		[Parameter] protected string Username { get; set; }

		[Parameter] protected bool IsAuthenticated { get; set; }

		[Inject] private IConfig _config { get; set; }

		protected NavBarItemComponent NavItemLogIn { get; set; }

		protected NavBarItemComponent NavItemProfile { get; set; }

		protected NavBarItemComponent NavItemChat { get; set; }

		protected NavBarItemComponent NavItemCounter { get; set; }

		public void SetActive(NavBarItemComponent item) {
			if (item != _selectedItem) {
				if (_selectedItem != default) {
					_selectedItem.IsActive = false;
				}
				_selectedItem = item;
				if (_selectedItem != default) {
					_selectedItem.IsActive = true;
				}
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