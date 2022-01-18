
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autobarn.Website.GraphQL.Queries {
    public class VehicleQuery : ObjectGraphType {
        private readonly IAutobarnDatabase db;
        public VehicleQuery(IAutobarnDatabase db) {
            this.db = db;

            Field<ListGraphType<VehicleGraphType>>("Vehicles", "Query to retrieve all Vehicles",
                resolve: GetAllVehicles);

            Field<ListGraphType<VehicleGraphType>>("VehiclesByColor", "Retrieve all vehicles of a particular color",
                   new QueryArguments(MakeNonNullStringArgument("color", "The name of a color, eg 'blue', 'grey'")),
                resolve: ListVehiclesByColor);
        }

        private object ListVehiclesByColor(IResolveFieldContext<object> context) {
            var color = context.GetArgument<string>("color");
            return db.ListVehicles()
                .Where(v => v.Color.Contains(color, StringComparison.InvariantCultureIgnoreCase));
        }

        private QueryArgument MakeNonNullStringArgument(string name, string description) {
            return new QueryArgument<NonNullGraphType<StringGraphType>> {
                Name = name, Description = description
            };
        }

        private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> arg) {
            return db.ListVehicles();
        }
    }
}