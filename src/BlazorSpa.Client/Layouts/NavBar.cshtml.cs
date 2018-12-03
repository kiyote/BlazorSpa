using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorSpa.Client.Pages.Auth;
using Microsoft.AspNetCore.Blazor.Components;

namespace BlazorSpa.Client.Layouts {
    public class NavBarComponent : BlazorComponent
    {
		[Parameter] protected string Username { get; set; }

		[Parameter] protected bool IsAuthenticated { get; set; }

		[Inject] private IConfig _config { get; set; }

		public string AuthUrl {
			get {
				return $"{_config.AuthUrl}&redirect_uri={_config.Host}{ValidateComponent.Url}";
			}
		}

	}
}