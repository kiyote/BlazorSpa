using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using BlazorSpa.Repository.DynamoDb.Model;
using BlazorSpa.Repository.Model;
using BlazorSpa.Shared;

namespace BlazorSpa.Repository.DynamoDb {
	public class StructureRepository : IStructureRepository {

		private readonly IDynamoDBContext _context;

		public StructureRepository(
			IDynamoDBContext context
		) {
			_context = context;
		}

		async Task<Structure> IStructureRepository.AddStructure(
			Id<Structure> structureId,
			string structureType,
			string name,
			DateTime dateCreated
		) {
			var sr = new StructureRecord() {
				StructureId = structureId.Value,
				StructureType = structureType,
				Name = name,
				DateCreated = dateCreated.ToUniversalTime()
			};

			await _context.SaveAsync( sr );

			return new Structure( structureId, structureType, name );
		}

		async Task IStructureRepository.AddChildStructure( 
			Id<View> viewId, 
			Id<Structure> parentStructureId, 
			Id<Structure> childStructureId, 
			DateTime dateCreated 
		) {
			var childRecord = new ChildStructureRecord() {
				ViewId = viewId.Value,
				ParentStructureId = parentStructureId.Value,
				ChildStructureId = childStructureId.Value,
				DateCreated = dateCreated.ToUniversalTime()
			};

			await _context.SaveAsync( childRecord );
		}

		async Task<IEnumerable<Id<Structure>>> IStructureRepository.GetChildStructureIds( 
			Id<View> viewId, 
			Id<Structure> structureId 
		) {
			var query = _context.QueryAsync<ChildStructureRecord>(
				StructureRecord.GetKey( structureId.Value ),
				QueryOperator.BeginsWith,
				new List<object>() { ChildStructureRecord.ChildStructureType( viewId.Value ) }
			);

			var childStructures = await query.GetRemainingAsync();
			var result = new List<Id<Structure>>();
			foreach( var childStructure in childStructures ) {
				result.Add( new Id<Structure>( childStructure.ChildStructureId ) );
			}

			return result;
		}

		async Task<IEnumerable<Structure>> IStructureRepository.GetStructures( IEnumerable<Id<Structure>> structureIds ) {
			var batchGet = _context.CreateBatchGet<StructureRecord>();
			foreach( var id in structureIds ) {
				batchGet.AddKey( StructureRecord.GetKey( id.Value ), StructureRecord.GetKey( id.Value ) );
			}
			await batchGet.ExecuteAsync();

			var result = new List<Structure>();
			foreach( var structure in batchGet.Results ) {
				result.Add( new Structure(
					new Id<Structure>( structure.StructureId ),
					structure.StructureType,
					structure.Name
				) );
			}

			return result.OrderBy( s => s.Name );
		}

		async Task<IEnumerable<View>> IStructureRepository.GetViews( IEnumerable<Id<View>> viewIds ) {
			var batchGet = _context.CreateBatchGet<ViewRecord>();
			foreach( var id in viewIds ) {
				batchGet.AddKey( ViewRecord.FixedViewId, ViewRecord.GetKey( id.Value ) );
			}
			await batchGet.ExecuteAsync();

			var result = new List<View>();
			foreach( var view in batchGet.Results ) {
				result.Add( new View(
					new Id<View>( view.ViewId ),
					view.ViewType,
					view.Name
				) );
			}

			return result.OrderBy( v => v.Name );
		}

		async Task<View> IStructureRepository.AddView( 
			Id<View> viewId, 
			string viewType, 
			string name, 
			DateTime dateCreated 
		) {
			var viewRecord = new ViewRecord() {
				ViewId = viewId.Value,
				ViewType = viewType,
				Name = name,
				DateCreated = dateCreated.ToUniversalTime()
			};

			await _context.SaveAsync( viewRecord );

			return new View( viewId, viewType, name );
		}

		async Task<IEnumerable<Id<View>>> IStructureRepository.GetViewIds() {
			var query = _context.QueryAsync<ViewRecord>(
				ViewRecord.FixedViewId,
				QueryOperator.BeginsWith,
				new List<object>() { ViewRecord.ViewItemType }
			);

			var views = await query.GetRemainingAsync();
			var result = new List<Id<View>>();
			foreach( var view in views ) {
				result.Add( new Id<View>( view.ViewId ) );
			}

			return result;
		}

		async Task<IEnumerable<Id<Structure>>> IStructureRepository.GetViewStructureIds( Id<View> viewId ) {
			var query = _context.QueryAsync<ViewStructureRecord>(
				ViewRecord.GetKey( viewId.Value ),
				QueryOperator.BeginsWith,
				new List<object>() { StructureRecord.StructureItemType }
			);

			var structures = await query.GetRemainingAsync();
			var result = new List<Id<Structure>>();
			foreach( var structure in structures ) {
				result.Add( new Id<Structure>( structure.StructureId ) );
			}

			return result;
		}

		async Task<StructureOperationStatus> IStructureRepository.AddViewStructure( 
			Id<View> viewId, 
			Id<Structure> structureId, 
			DateTime dateCreated 
		) {
			var record = new ViewStructureRecord() {
				ViewId = viewId.Value,
				StructureId = structureId.Value,
				DateCreated = dateCreated.ToUniversalTime()
			};

			await _context.SaveAsync( record );

			return StructureOperationStatus.Success;
		}

		async Task<Id<Structure>> IStructureRepository.GetParentStructureId( 
			Id<View> viewId, 
			Id<Structure> structureId 
		) {
			var query = _context.QueryAsync<ViewParentStructureRecord>(
				ChildStructureRecord.GetKey( viewId.Value, structureId.Value ),
				QueryOperator.BeginsWith,
				new List<object>() { StructureRecord.StructureItemType }, 
				new DynamoDBOperationConfig() {
					IndexName = "GSI"
				}
			);

			var structures = await query.GetRemainingAsync();
			var parentStructure = structures.FirstOrDefault();

			if (parentStructure == default) {
				return default;
			}

			return new Id<Structure>( parentStructure.StructureId );
		}
	}
}