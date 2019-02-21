using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorSpa.Client.Model;
using BlazorSpa.Client.Services;
using Microsoft.AspNetCore.Blazor.Components;

namespace BlazorSpa.Client.Pages.Admin {
	public class OrganizationComponent : BlazorComponent
    {
		public static string Url = "/admin/organization";

		[Inject] private IStructureApiService _structureService { get; set; }

		protected string _createViewName { get; set; }

		public IEnumerable<View> Organizations { get; set; } = new List<View>();

		public bool CreatingView { get; set; }

		public bool Busy { get; set; }

		async protected override Task OnInitAsync() {
			Organizations = await _structureService.GetUserViews();
		}

		public void ShowCreateView() {
			CreatingView = true;
		}

		async public Task CreateView() {
			Busy = true;
			await _structureService.CreateView( "Organization", _createViewName );
			Organizations = await _structureService.GetUserViews();
			CreatingView = false;
			Busy = false;
		}
	}
}