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
