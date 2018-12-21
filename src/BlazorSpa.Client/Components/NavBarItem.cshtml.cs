using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;

namespace BlazorSpa.Client.Components {
	public class NavBarItemComponent: BlazorComponent {
		[Parameter]
		protected RenderFragment ChildContent { get; set; }

		[Parameter]
		internal bool IsSelected { get; set; }
	}
}
