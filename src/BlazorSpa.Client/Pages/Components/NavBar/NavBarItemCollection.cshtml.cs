using Microsoft.AspNetCore.Components;

namespace BlazorSpa.Client.Pages.Components.NavBar {
	public class NavBarItemCollectionComponent : ComponentBase {

		[Parameter]
		protected RenderFragment ChildContent { get; set; }

		[Parameter]
		protected NavBarItemsLayout Layout { get; set; }

	}
}
