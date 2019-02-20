using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorSpa.Client.Services;
using BlazorSpa.Model;
using Microsoft.AspNetCore.Blazor.Components;

namespace BlazorSpa.Client.Pages.Admin
{
    public class OrganizationComponent : BlazorComponent
    {
		public static string Url = "/admin/organization";

		[Inject] private IStructureApiService _structureService { get; set; }

		protected string _createViewName { get; set; }

		public IEnumerable<ApiView> Organizations { get; set; } = new List<ApiView>();

		public bool CreatingView { get; set; }

		async protected override Task OnInitAsync() {
			Organizations = await _structureService.GetUserViews();
		}

		public void ShowCreateView() {
			CreatingView = true;
		}

		async public Task CreateView() {
			await _structureService.CreateView( "Organization", _createViewName );
			Organizations = await _structureService.GetUserViews();
			CreatingView = false;
		}
	}
}