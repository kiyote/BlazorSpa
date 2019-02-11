using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorSpa.Repository;
using BlazorSpa.Repository.Model;
using Moq;
using NUnit.Framework;

namespace BlazorSpa.Service.Tests {
	public class StructureServiceTests {

		private IStructureService _service;
		private Mock<IStructureRepository> _structureRepository;

		[SetUp]
		public void Setup() {
			_structureRepository = new Mock<IStructureRepository>( MockBehavior.Strict );
			_service = new StructureService( _structureRepository.Object );
		}

		[Test]
		public void GetHomeStructures_NoHomeStructures_EmptyListRetrieved() {
			var userId = new Id<User>();
			IEnumerable<Id<Structure>> structureIds = new List<Id<Structure>>();
			IEnumerable<Structure> structures = new List<Structure>();
			_structureRepository
				.Setup( r => r.GetHomeStructureIds( userId ) )
				.Returns( Task.FromResult( structureIds ));
			_structureRepository
				.Setup( r => r.GetStructures( structureIds ) )
				.Returns( Task.FromResult( structures ));

			var actual = _service.GetHomeStructures( userId );

			_structureRepository.VerifyAll();
		}
	}
}