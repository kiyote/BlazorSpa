using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using BlazorSpa.Repository.DynamoDb.Model;
using BlazorSpa.Repository.Model;

namespace BlazorSpa.Repository.DynamoDb {
	public class StructureRepository : IStructureRepository {

		private readonly IDynamoDBContext _context;

		public StructureRepository(
			IDynamoDBContext context
		) {
			_context = context;
		}

		async Task<Structure> IStructureRepository.Add( 
			Id<Structure> structureId, 
			string structureType 
		) {
			var sr = new StructureRecord() {
				StructureId = structureId.Value,
				StructureType = structureType,
				Status = StructureRecord.Active
			};

			await _context.SaveAsync( sr );

			return new Structure( structureId, structureType );
		}
	}
}
