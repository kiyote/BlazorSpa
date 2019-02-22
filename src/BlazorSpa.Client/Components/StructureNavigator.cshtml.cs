using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorSpa.Client.Model;
using BlazorSpa.Client.Services;
using BlazorSpa.Shared;
using Microsoft.AspNetCore.Blazor.Components;

namespace BlazorSpa.Client.Components {
	public class StructureNavigatorComponent : BlazorComponent {

		[Inject] private IStructureApiService _structureService { get; set; }

		protected View SelectedView { get; set; }

		protected string _createStructureName { get; set; }

		protected IEnumerable<Structure> Structures { get; set; } = new List<Structure>();

		[Parameter] protected bool AllowCreate { get; set; }

		[Parameter] protected string StructureType { get; set; }

		protected bool CreatingStructure { get; set; }

		protected bool Busy { get; set; }

		public async Task SelectedViewChanged( View view ) {
			SelectedView = view;
			await LoadStructures( view.Id );
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
			await _structureService.CreateView( StructureType, _createStructureName );
			await LoadStructures( SelectedView.Id );
			CreatingStructure = false;
			Busy = false;
		}

		private async Task LoadStructures( Id<View> viewId ) {
			Structures = await _structureService.GetViewStructures( viewId );
		}
	}
}
