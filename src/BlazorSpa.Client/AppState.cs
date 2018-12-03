using System;
using Cloudcrate.AspNetCore.Blazor.Browser.Storage;

namespace BlazorSpa.Client {
	public class AppState {

		private readonly SessionStorage _storage;

		public AppState(SessionStorage storage) {
			_storage = storage;
			_storage[ "TokensExpireAt" ] = DateTimeOffset.MinValue.ToString( "o" );
		}

		public event EventHandler OnStateChanged;

		public string Session {
			get {
				return _storage[ "Session" ];
			}
			set {
				_storage[ "Session" ] = value;
			}
		}

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

		public DateTimeOffset TokensExpireAt {
			get {
				return DateTimeOffset.Parse( _storage[ "TokensExpireAt" ] );
			}
			set {
				_storage[ "TokensExpireAt" ] = value.ToString( "o" );
				OnStateChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public bool IsAuthenticated {
			get {
				return ( TokensExpireAt > DateTimeOffset.Now );
			}
		}
	}
}
