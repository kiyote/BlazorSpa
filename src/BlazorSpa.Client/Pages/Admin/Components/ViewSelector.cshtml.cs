/*
 * Copyright 2018-2019 Todd Lang
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorSpa.Client.Model;
using BlazorSpa.Client.Services;
using BlazorSpa.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorSpa.Client.Pages.Admin.Components {
	public class ViewSelectorComponent : ComponentBase {

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

		async protected Task SelectedViewChanged( UIChangeEventArgs e ) {
			if( Views != default ) {
				var viewId = new Id<View>( e.Value.ToString() );
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
