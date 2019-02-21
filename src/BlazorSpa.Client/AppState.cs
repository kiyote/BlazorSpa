using System;
using Cloudcrate.AspNetCore.Blazor.Browser.Storage;

namespace BlazorSpa.Client {
	public class AppState {

		private readonly SessionStorage _storage;

		public AppState( SessionStorage storage ) {
			_storage = storage;

			if( _storage[ "TokensExpireAt" ] == default ) {
				_storage[ "TokensExpireAt" ] = DateTime.MinValue.ToUniversalTime().ToString( "o" );
			}
		}

		public event EventHandler OnStateChanged;

		public string Username {
			get {
				return _storage[ "Username" ];
			}
			set {
				_storage[ "Username" ] = value;
				OnStateChanged?.Invoke( this, EventArgs.Empty );
			}
		}

		public string AccessToken {
			get {
				return _storage[ "AccessToken" ];
			}
			set {
				_storage[ "AccessToken" ] = value;
			}
		}

		public string RefreshToken {
			get {
				return _storage[ "RefreshToken" ];
			}
			set {
				_storage[ "RefeshToken" ] = value;
			}
		}

		public DateTime TokensExpireAt {
			get {
				return DateTime.Parse( _storage[ "TokensExpireAt" ] ).ToUniversalTime();
			}
			set {
				_storage[ "TokensExpireAt" ] = value.ToString( "o" );
				OnStateChanged?.Invoke( this, EventArgs.Empty );
			}
		}

		public bool IsAuthenticated {
			get {
				var result = ( TokensExpireAt > DateTime.UtcNow );
				return result;
			}
		}
	}
}