using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorSpa.Client.Components {
	public class CardComponent : ComponentBase, IDisposable {

		[Inject] private IJSRuntime _js { get; set; }

		[Parameter] protected string Id { get; set; }

		protected override async Task OnAfterRenderAsync() {
			await _js.InvokeAsync<object>( "dragDrop.registerDraggable", Id, new DotNetObjectRef( this ) );
		}

		[JSInvokable]
		public void OnDragStarted() {
			Console.WriteLine( $"Drag Started {Id}" );
		}

		[JSInvokable]
		public void OnDragEnded() {
			Console.WriteLine( $"Drag Ended {Id}" );
		}

		void IDisposable.Dispose() {
			_js.InvokeAsync<object>( "dragDrop.unregisterDraggable", Id ).ConfigureAwait(false);
		}
	}
}
