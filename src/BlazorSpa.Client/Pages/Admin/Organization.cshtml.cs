using System.Threading.Tasks;
using BlazorSpa.Client.Components;
using BlazorSpa.Client.Model;
using Microsoft.AspNetCore.Components;

namespace BlazorSpa.Client.Pages.Admin {
	public class OrganizationComponent : ComponentBase {
		public static string Url = "/admin/organization";

		protected StructureNavigatorComponent StructureNavigator { get; set; }
		protected ViewSelectorComponent ViewSelector { get; set; }

		public async Task SelectedViewChanged( View view ) {
			await StructureNavigator.SelectedViewChanged( view );
		}
	}
}
