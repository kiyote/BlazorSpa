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
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorSpa.Client.Components
{
    public class CardContainerComponent : ComponentBase, IDisposable
    {
		[Parameter] protected RenderFragment ChildContent { get; set; }

		[Inject] private IJSRuntime _js { get; set; }

		[Parameter] protected string Id { get; set; }

		protected override async Task OnAfterRenderAsync() {
			await _js.InvokeAsync<object>( "dragDrop.registerDroppable", Id, new DotNetObjectRef( this ) );
		}

		[JSInvokable]
		public void OnDropped(string id) {
			Console.WriteLine( $"{Id} received {id}" );
		}

		void IDisposable.Dispose() {
			_js.InvokeAsync<object>( "dragDrop.unregisterDroppable", Id ).ConfigureAwait( false );
		}
	}
}
