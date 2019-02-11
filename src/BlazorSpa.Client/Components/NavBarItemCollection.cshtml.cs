using System;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;

namespace BlazorSpa.Client.Components
{
    public class NavBarItemCollectionComponent : BlazorComponent {

		[Parameter]
		protected RenderFragment ChildContent { get; set; }

		[Parameter]
		protected NavBarItemsLayout Layout { get; set; }

	}
}