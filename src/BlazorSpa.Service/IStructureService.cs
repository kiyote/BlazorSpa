/*
 * Copyright 2018-2019 Todd Lang
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorSpa.Repository;
using BlazorSpa.Repository.Model;
using BlazorSpa.Shared;

namespace BlazorSpa.Service {
	public interface IStructureService {
		Task<IEnumerable<View>> GetUserViews( Id<User> userId );

		Task<IEnumerable<View>> GetAllViews();

		Task<IEnumerable<Structure>> GetViewRootStrutures( Id<View> viewId );

		Task<Structure> CreateStructure( Id<Structure> structureId, string structureType, string name, DateTime createdOn );

		Task<StructureOperationStatus> AddStructureToView( Id<Structure> structureId, Id<View> viewId, DateTime createdOn );

		Task<View> CreateViewWithUser( Id<User> userId, Id<View> viewId, string viewType, string viewName, DateTime createdOn );

		Task<IEnumerable<Structure>> GetChildStructures( Id<View> viewId, Id<Structure> structureId );

		Task AddChildStructure( Id<View> viewId, Id<Structure> parentStructureId, Id<Structure> structureId, DateTime createdOn );

		Task<Structure> GetParentStructure( Id<View> viewId, Id<Structure> structureId );
	}
}
