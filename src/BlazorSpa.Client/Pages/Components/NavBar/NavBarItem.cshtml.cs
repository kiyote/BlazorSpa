﻿using Microsoft.AspNetCore.Components;

namespace BlazorSpa.Client.Pages.Components.NavBar {
	public class NavBarItemComponent : ComponentBase {

		[Parameter] protected RenderFragment ChildContent { get; set; }

		internal bool IsSelected { get; set; }

		internal bool IsActive { get; set; }

		[CascadingParameter] internal NavBarComponent NavBar { get; set; }

		public ElementRef ListItem { get; set; }
	}
}