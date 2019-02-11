using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlazorSpa.Repository.Model;

namespace BlazorSpa.Service {
	public interface IStructureService {
		Task<IEnumerable<Structure>> GetHomeStructures( Id<User> userId );
	}
}
