using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorSpa.Repository {
	public enum AuthenticationStatus {
		Unknown,

		Failure,

		MustChangePassword,

		Success
	}
}
