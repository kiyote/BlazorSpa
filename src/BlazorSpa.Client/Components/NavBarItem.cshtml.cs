using BlazorSpa.Client.Layouts;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;

namespace BlazorSpa.Client.Components {
	public class NavBarItemComponent : BlazorComponent {

		[Parameter]
		protected RenderFragment ChildContent { get; set; }

		internal bool IsSelected { get; set; }

		internal bool IsActive { get; set; }

		[CascadingParameter]
		internal NavBarComponent NavBar { get; set; }
	}
}