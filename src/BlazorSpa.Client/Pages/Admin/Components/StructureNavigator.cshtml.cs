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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorSpa.Client.Model;
using BlazorSpa.Client.Services;
using BlazorSpa.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorSpa.Client.Pages.Admin.Components {
	public class StructureNavigatorComponent : ComponentBase {

		[Inject] private IStructureApiService _structureService { get; set; }

		protected View SelectedView { get; set; }

		protected Structure SelectedStructure { get; set; }

		protected string _createStructureName { get; set; }

		protected IEnumerable<Structure> Structures { get; set; } = new List<Structure>();

		[Parameter] protected bool AllowCreate { get; set; }

		[Parameter] protected string StructureType { get; set; }

		protected bool CreatingStructure { get; set; }

		protected bool Busy { get; set; }

		public async Task SelectedViewChanged( View view ) {
			SelectedView = view;
			await LoadViewStructures( view.Id );
			StateHasChanged();
		}

		protected void EnterCreateStructure() {
			CreatingStructure = true;
		}

		protected void CancelCreateStructure() {
			CreatingStructure = false;
		}

		protected async Task CreateStructure() {
			Busy = true;
			if( SelectedStructure != default ) {
				await _structureService.CreateChildStructure( SelectedView.Id, SelectedStructure.Id, StructureType, _createStructureName );
				Structures = await _structureService.GetChildStructures( SelectedView.Id, SelectedStructure.Id );
			} else {
				await _structureService.CreateStructureInView( SelectedView.Id, StructureType, _createStructureName );
				await LoadViewStructures( SelectedView.Id );
			}
			_createStructureName = default;
			CreatingStructure = false;
			Busy = false;
		}

		protected async Task SelectStructure( Id<Structure> structureId ) {
			Busy = true;
			SelectedStructure = Structures.First( s => s.Id == structureId );
			Structures = await _structureService.GetChildStructures( SelectedView.Id, structureId );
			Busy = false;
		}

		protected async Task SelectParentStructure( Id<Structure> structureId ) {
			Busy = true;
			if( structureId == default ) {
				SelectedStructure = default;
				await LoadViewStructures( SelectedView.Id );
			} else {
				SelectedStructure = await _structureService.GetParentStructure( SelectedView.Id, structureId );
				if( SelectedStructure != default ) {
					Structures = await _structureService.GetChildStructures( SelectedView.Id, SelectedStructure.Id );
				} else {
					Structures = await _structureService.GetViewStructures( SelectedView.Id );
				}
			}
			Busy = false;
		}

		private async Task LoadViewStructures( Id<View> viewId ) {
			Busy = true;
			Structures = await _structureService.GetViewStructures( viewId );
			Busy = false;
		}
	}
}
