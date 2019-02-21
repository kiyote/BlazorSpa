using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorSpa.Client.Model;
using BlazorSpa.Client.Services;
using BlazorSpa.Shared;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;

namespace BlazorSpa.Client.Pages.Admin {
	public class OrganizationComponent : BlazorComponent
    {
		public static string Url = "/admin/organization";

		[Inject] private IStructureApiService _structureService { get; set; }

		protected string _createViewName { get; set; }

		protected string _createLocationName { get; set; }

		private Id<View> _selectedViewId;

		public IEnumerable<View> Organizations { get; private set; } = new List<View>();

		public IEnumerable<Structure> Locations { get; private set; } = new List<Structure>();

		public bool CreatingView { get; private set; }

		public bool CreatingLocation { get; private set; }

		public bool Busy { get; private set; }

		async protected override Task OnInitAsync() {
			Busy = true;
			Organizations = await _structureService.GetUserViews();
			if (Organizations.Any()) {
				_selectedViewId = Organizations.First().Id;
				await LoadStructures( _selectedViewId );
			} else {
				_selectedViewId = default;
			}
			Busy = false;
		}

		public void ShowCreateView() {
			CreatingView = true;
		}

		public void HideCreateView() {
			CreatingView = false;
		}

		public void ShowCreateLocation() {
			CreatingLocation = true;
		}

		public void HideCreateLocation() {
			CreatingLocation = false;
		}

		async public Task CreateOrganization() {
			Busy = true;
			await _structureService.CreateView( "Organization", _createViewName );
			Organizations = await _structureService.GetUserViews();
			CreatingView = false;
			Busy = false;
		}

		async public Task OrganizationSelected(UIChangeEventArgs e) {
			Busy = true;
			var viewId = new Id<View>(e.Value.ToString());
			_selectedViewId = viewId;
			await LoadStructures( viewId );
			Busy = false;
		}

		private async Task LoadStructures(Id<View> viewId) {
			Locations = await _structureService.GetViewStructures( viewId );
		}

		async public Task CreateLocation() {
			Busy = true;
			await _structureService.CreateStructureInView( _selectedViewId, "Location", _createLocationName );
			Locations = await _structureService.GetViewStructures( _selectedViewId );
			CreatingLocation = false;
			Busy = false;
		}
	}
}