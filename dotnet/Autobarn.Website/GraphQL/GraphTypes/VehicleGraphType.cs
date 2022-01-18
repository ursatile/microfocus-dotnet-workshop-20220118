using Autobarn.Data.Entities;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.GraphTypes {
    public sealed class VehicleGraphType : ObjectGraphType<Vehicle> {
        public VehicleGraphType() {
            Name = "vehicle";
            Field(c => c.VehicleModel, nullable: false, type: typeof(VehicleModelGraphType))
                .Description("The model of this particular vehicle");
            Field(c => c.Registration).Description("The registration number (licence plate) of this vehicle");
            Field(c => c.Color).Description("What color is this vehicle?");
            Field(c => c.Year).Description("The year that this vehicle was first registered");
        }
    }
}