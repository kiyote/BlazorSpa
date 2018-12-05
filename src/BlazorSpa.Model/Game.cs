using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorSpa.Model {
	public class Game {
		public Id<Game> Id { get; }

		public DateTimeOffset CreatedOn { get; }

		public Id<User> CreatedBy { get; }
	}
}
