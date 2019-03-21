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
using System.Threading.Tasks;
using BlazorSpa.Client.Components;
using BlazorSpa.Client.Model;
using Microsoft.AspNetCore.Components;
using BlazorSpa.Client.Pages.Admin.Components;

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
