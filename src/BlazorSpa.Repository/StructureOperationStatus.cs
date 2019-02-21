using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BlazorSpa.Repository {
	[JsonConverter( typeof( StringEnumConverter ) )]
	public enum StructureOperationStatus {
		Unknown,

		Failure, 

		Success,

		AlreadyExists
	}

}
