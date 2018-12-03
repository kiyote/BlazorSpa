using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorSpa.Model.Api {
	public class UserInformationRequest {

		public UserInformationRequest() {
			Username = string.Empty;
		}

		public string Username { get; set; }
	}
}
