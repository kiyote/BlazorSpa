
using System;
using Microsoft.AspNetCore.Blazor.Components;

namespace BlazorSpa.Client.Layouts {
	public class NavMenuComponent : BlazorComponent {

		[Parameter] protected bool IsAuthenticated { get; set; }
	}
}