using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorSpa.Client.Model;
using BlazorSpa.Client.Services;
using BlazorSpa.Shared;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;

namespace BlazorSpa.Client.Components {
	public class ViewSelectorComponent: BlazorComponent {

		[Inject] private IStructureApiService _structureService { get; set; }

		[Parameter] protected bool AllowCreate { get; set; }

		[Parameter] protected string ViewType { get; set; }

		[Parameter] protected Func<View, Task> OnSelectionChanged { get; set; }

		protected string _createViewName { get; set; }

		protected View SelectedView { get; set; }

		protected IEnumerable<View> Views { get; set; } = new List<View>();

		protected bool CreatingView { get; set; }

		protected bool Busy { get; set; }

		public ViewSelectorComponent() {
		}

		async protected override Task OnInitAsync() {
			Busy = true;
			await LoadViews();
			Busy = false;
		}

		async protected Task SelectedViewChanged( UIChangeEventArgs e) {
			if (Views != default) {
				var viewId = new Id<View>(e.Value.ToString());
				SelectedView = Views.First( v => v.Id == viewId );
				await OnSelectionChanged.Invoke( SelectedView );
			}
		}

		protected void EnterCreateMode() {
			CreatingView = true;
		}

		protected void CancelCreateMode() {
			CreatingView = false;
		}

		protected async Task CreateView() {
			Busy = true;
			await _structureService.CreateView( ViewType, _createViewName );
			await LoadViews();
			_createViewName = default;
			CreatingView = false;
			Busy = false;
		}

		private async Task LoadViews() {
			Views = await _structureService.GetUserViews();
			if( SelectedView == default ) {
				if( Views.Any() ) {
					SelectedView = Views.First();
				} else {
					SelectedView = default;
				}
			}
			await OnSelectionChanged.Invoke( SelectedView );
		}
	}
}
