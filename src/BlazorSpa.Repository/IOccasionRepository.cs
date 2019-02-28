using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlazorSpa.Repository.Model;
using BlazorSpa.Shared;

namespace BlazorSpa.Repository {
	interface IOccasionRepository {

		Task<Occasion> CreateOccasion( Id<Occasion> id, DateTime occursOn, string name );
	}
}
