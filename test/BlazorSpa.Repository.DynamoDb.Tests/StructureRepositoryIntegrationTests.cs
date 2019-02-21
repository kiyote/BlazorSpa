using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using BlazorSpa.Repository.Model;
using BlazorSpa.Shared;
using NUnit.Framework;

namespace BlazorSpa.Repository.DynamoDb.Tests {
	[TestFixture]
	internal sealed class StructureRepositoryIntegrationTests {

		private IStructureRepository _structureRepository;
		private IAmazonDynamoDB _client;

		[OneTimeSetUp]
		public void OneTimeSetUp() {
			var config = new AmazonDynamoDBConfig() {
				UseHttp = true,
				ServiceURL = @"http://localhost:8000"
			};
			var credentials = new BasicAWSCredentials( "fake", "fake" );
			_client = new AmazonDynamoDBClient( credentials, config );
			var context = new DynamoDBContext( _client );

			_structureRepository = new StructureRepository( context );
		}

		[SetUp]
		public async Task SetUp() {
			await RemoveTable( _client );
			await CreateTable( _client );
		}

		[Test]
		public async Task AddStructure_ValidData_StructureReturned() {
			var structureId = new Id<Structure>();
			var structure = await _structureRepository.AddStructure( structureId, "test", "name", DateTime.UtcNow );

			Assert.IsNotNull( structure );
			Assert.AreEqual( structureId, structure.Id );
		}

		[Test]
		public async Task GetStructures_OneValidStructure_StructureReturned() {
			var structureId = new Id<Structure>();
			var structureType = "test";
			var structureName = "name";
			var createdOn = DateTime.UtcNow;
			await _structureRepository.AddStructure( structureId, structureType, structureName, createdOn );

			var structures = await _structureRepository.GetStructures( new List<Id<Structure>>() { structureId } );
			var structure = structures.FirstOrDefault();

			Assert.IsNotNull( structure );
			Assert.AreEqual( structureId, structure.Id );
			Assert.AreEqual( structureType, structure.StructureType );
			Assert.AreEqual( structureName, structure.Name );
		}

		[Test]
		public async Task GetStructures_TwoStructuresOnRequested_StructureReturned() {
			var structure1 = await CreateStructure( _structureRepository, "test1", "name" );
			var structure2 = await CreateStructure( _structureRepository, "test2", "name" );

			var structures = await _structureRepository.GetStructures( new List<Id<Structure>>() { structure2.Id } );
			var structure = structures.FirstOrDefault();

			Assert.IsNotNull( structure );
			Assert.AreEqual( structure2.Id, structure.Id );
			Assert.AreEqual( structure2.StructureType, structure.StructureType );
		}

		[Test]
		public async Task AddChild_OneChild_NoErrors() {
			var viewId = new Id<View>();
			var structure1 = await CreateStructure( _structureRepository, "test1", "name" );
			var structure2 = await CreateStructure( _structureRepository, "test2", "name" );
			var dateCreated = DateTime.UtcNow;

			await _structureRepository.AddChildStructure( viewId, structure1.Id, structure2.Id, dateCreated );
		}

		[Test]
		public async Task GetChildStructureIds_OneChild_OneChildReturned() {
			var viewId = new Id<View>();
			var structure1 = await CreateStructure( _structureRepository, "test1", "name" );
			var structure2 = await CreateStructure( _structureRepository, "test2", "name" );
			var dateCreated = DateTime.UtcNow;
			await _structureRepository.AddChildStructure( viewId, structure1.Id, structure2.Id, dateCreated );

			var children = await _structureRepository.GetChildStructureIds( viewId, structure1.Id );

			Assert.AreEqual( 1, children.Count() );
			CollectionAssert.Contains( children, structure2.Id );
		}

		[Test]
		public async Task AddView_ValidData_ViewReturned() {
			var viewId = new Id<View>();
			var view = await _structureRepository.AddView( viewId, "type", "test", DateTime.UtcNow );

			Assert.IsNotNull( view );
			Assert.AreEqual( viewId, view.Id );
		}

		[Test]
		public async Task GetViewIds_OneView_OneViewReturned() {
			var viewId = new Id<View>();
			var view = await _structureRepository.AddView( viewId, "type", "test", DateTime.UtcNow );

			var views = await _structureRepository.GetViewIds();
			Assert.AreEqual( 1, views.Count() );
			CollectionAssert.Contains( views, viewId );
		}

		[Test]
		public async Task GetViews_NoViews_EmptyListReturned() {
			var viewId = new Id<View>();
			var viewIds = new List<Id<View>>() { viewId };

			var views = await _structureRepository.GetViews( viewIds );

			Assert.IsNotNull( views );
			CollectionAssert.IsEmpty( views );
		}

		[ Test ]
		public async Task GetViews_OneView_OneViewReturned() {
			var viewId = new Id<View>();
			var view = await _structureRepository.AddView( viewId, "type", "test", DateTime.UtcNow );
			var viewIds = new List<Id<View>>() { viewId };

			var views = await _structureRepository.GetViews( viewIds );

			Assert.AreEqual( 1, views.Count() );
			CollectionAssert.Contains( views, view );
		}

		[Test]
		public async Task AddViewStructure_ValidViewValidStructure_NoErrors() {
			var viewId = new Id<View>();
			await _structureRepository.AddView( viewId, "type", "test", DateTime.UtcNow );
			var structureId = new Id<Structure>();

			await _structureRepository.AddViewStructure( viewId, structureId, DateTime.UtcNow );
		}

		[Test]
		public async Task GetViewStructureIds_ValidViewOneStructure_OneStructureReturned() {
			var viewId = new Id<View>();
			var structureId = new Id<Structure>();
			var expected = new List<Id<Structure>>() {
				structureId
			};
			await _structureRepository.AddView( viewId, "type", "test", DateTime.UtcNow );
			await _structureRepository.AddViewStructure( viewId, structureId, DateTime.UtcNow );

			var actual = await _structureRepository.GetViewStructureIds( viewId );
			CollectionAssert.AreEquivalent( expected, actual );
		}

		private static async Task<Structure> CreateStructure( 
			IStructureRepository repository, 
			string structureType,
			string name
		) {
			var structureId = new Id<Structure>();
			var createdOn = DateTime.UtcNow;
			return await repository.AddStructure( structureId, structureType, name, createdOn );
		}

		private static async Task RemoveTable( IAmazonDynamoDB client ) {
			var deleteRequest = new DeleteTableRequest() {
				TableName = "BlazorSpa"
			};
			try {
				await client.DeleteTableAsync( deleteRequest );
			} catch( Exception ) {
			}
		}

		private static async Task CreateTable( IAmazonDynamoDB client ) {
			var request = new CreateTableRequest() {
				TableName = "BlazorSpa",
				AttributeDefinitions = new List<AttributeDefinition>() {
					new AttributeDefinition() {
						AttributeName = "PK",
						AttributeType = ScalarAttributeType.S
					},
					new AttributeDefinition() {
						AttributeName = "SK",
						AttributeType = ScalarAttributeType.S
					}
				},
				KeySchema = new List<KeySchemaElement>() {
					new KeySchemaElement() {
						AttributeName = "PK",
						KeyType = KeyType.HASH
					},
					new KeySchemaElement() {
						AttributeName = "SK",
						KeyType = KeyType.RANGE
					}
				},
				ProvisionedThroughput = new ProvisionedThroughput() {
					ReadCapacityUnits = 1,
					WriteCapacityUnits = 1
				},
				GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>() {
					new GlobalSecondaryIndex() {
						IndexName = "GSI",
						KeySchema = new List<KeySchemaElement>() {
							new KeySchemaElement() {
								AttributeName = "SK",
								KeyType = KeyType.HASH
							},
							new KeySchemaElement() {
								AttributeName = "PK",
								KeyType = KeyType.RANGE
							}
						},
						Projection = new Projection() {
							ProjectionType = ProjectionType.KEYS_ONLY
						},
						ProvisionedThroughput = new ProvisionedThroughput() {
							ReadCapacityUnits = 1,
							WriteCapacityUnits = 1
						}
					}
				}
			};
			await client.CreateTableAsync( request );
		}
	}
}