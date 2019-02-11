using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
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

		async Task IStructureRepository.Organize( Id<Structure> parentStructureId, Id<Structure> childStructureId) {
			/*
			var or = new OrganizationRecord() {
				StructureId = parentStructureId.Value,
				ChildStructureId = childStructureId.Value,
				DateCreated = DateTime.UtcNow,
				Status = OrganizationRecord.Active
			};

			await _context.SaveAsync( or );
			*/
		}

		async Task<IEnumerable<Id<Structure>>> IStructureRepository.GetHomeStructureIds( Id<User> userId) {
			var userKey = UserRecord.GetKey( userId.Value );
			var search = _context.QueryAsync<HomeStructureRecord>(
				userKey,
				QueryOperator.BeginsWith,
				new List<object>() { StructureRecord.StructureItemType } );

			var homeRecords = await search.GetRemainingAsync();

			var result = new List<Id<Structure>>();
			foreach (var homeRecord in homeRecords) {
				result.Add( new Id<Structure>( homeRecord.StructureId ) );
			}

			return result;
		}

		async Task<IEnumerable<Structure>> IStructureRepository.GetStructures(IEnumerable<Id<Structure>> structureIds) {
			var batchGet = _context.CreateBatchGet<StructureRecord>();
			foreach (var id in structureIds) {
				batchGet.AddKey( StructureRecord.GetKey( id.Value ) );
			}
			await batchGet.ExecuteAsync();

			var result = new List<Structure>();
			foreach (var structure in batchGet.Results) {
				result.Add( new Structure(
					new Id<Structure>(structure.StructureId),
					structure.StructureType
				) );
			}

			return result;
		}
	}
}
