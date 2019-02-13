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
			string structureType,
			DateTimeOffset dateCreated
		) {
			var sr = new StructureRecord() {
				StructureId = structureId.Value,
				StructureType = structureType,				
				Status = StructureRecord.Active,
				DateCreated = dateCreated.UtcDateTime
			};

			await _context.SaveAsync( sr );

			return new Structure( structureId, structureType );
		}

		async Task IStructureRepository.AddChild( Id<View> viewId, Id<Structure> parentStructureId, Id<Structure> childStructureId, DateTimeOffset dateCreated ) {
			var childRecord = new ChildStructureRecord() {
				ViewId = viewId.Value,
				ParentStructureId = parentStructureId.Value,
				ChildStructureId = childStructureId.Value,
				DateCreated = dateCreated.UtcDateTime
			};

			await _context.SaveAsync( childRecord );
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

		async Task<IEnumerable<Id<View>>> IStructureRepository.GetUserViewIds( Id<User> userId ) {
			var query = _context.QueryAsync<UserViewRecord>(
				UserRecord.GetKey( userId.Value ),
				QueryOperator.BeginsWith,
				new List<object>() { ViewRecord.ViewItemType } );

			var records = await query.GetRemainingAsync();

			var result = new List<Id<View>>();
			foreach (var record in records) {
				result.Add( new Id<View>( record.ViewId ) );
			}

			return result;
		}

		async Task<IEnumerable<View>> IStructureRepository.GetViews( IEnumerable<Id<View>> viewIds ) {
			var batchGet = _context.CreateBatchGet<ViewRecord>();
			foreach( var id in viewIds ) {
				batchGet.AddKey( ViewRecord.GetKey( id.Value ) );
			}
			await batchGet.ExecuteAsync();

			var result = new List<View>();
			foreach( var view in batchGet.Results ) {
				result.Add( new View(
					new Id<View>( view.ViewId ),
					view.ViewType
				) );
			}

			return result;
		}
	}
}
