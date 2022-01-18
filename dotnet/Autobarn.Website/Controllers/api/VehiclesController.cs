using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Messages;
using Autobarn.Website.Models;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Autobarn.Website.Controllers.api {
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase {
        private readonly IAutobarnDatabase db;
        private readonly IBus bus;

        public VehiclesController(IAutobarnDatabase db, IBus bus) {
            this.bus = bus;
            this.db = db;
        }

        const int PAGE_SIZE = 10;
        // GET: api/vehicles
        [HttpGet]
        [Produces("application/hal+json")]
        public IActionResult Get(int index = 0) {
            var items = db.ListVehicles().Skip(index).Take(PAGE_SIZE);
            var total = db.CountVehicles();
            dynamic _links = new ExpandoObject();
            _links.self = new {
                href = "/api/vehicles"
            };
            if (index > 0) {
                _links.prev = new {
                    href = $"/api/vehicles?index={index - PAGE_SIZE}"
                };
            }
            if (index < total) {
                _links.next = new {
                    href = $"/api/vehicles?index={index + PAGE_SIZE}"
                };
            }
            var result = new {
                _links,
                count = PAGE_SIZE,
                index,
                total,
                items
            };
            return Ok(result);
        }

        // GET api/vehicles/ABC123
        [HttpGet("{id}")]
        public IActionResult Get(string id) {
            var vehicle = db.FindVehicle(id);
            if (vehicle == default) return NotFound();
            return Ok(vehicle);
        }

        // POST api/vehicles
        [HttpPost]
        public IActionResult Post([FromBody] VehicleDto dto) {
            var vehicleModel = db.FindModel(dto.ModelCode);
            var vehicle = new Vehicle {
                Registration = dto.Registration,
                Color = dto.Color,
                Year = dto.Year,
                VehicleModel = vehicleModel
            };
            db.CreateVehicle(vehicle);
            PublishNewVehicleNotification(vehicle);
            return Created($"/api/vehicles/{vehicle.Registration}", dto);
        }

        private void PublishNewVehicleNotification(Vehicle vehicle) {
            var message = new NewVehicleMessage {
                Color = vehicle.Color,
                ManufacturerName = vehicle.VehicleModel?.Manufacturer?.Name ?? "(unknown)",
                ModelName = vehicle.VehicleModel?.Name ?? "(unknown)",
                Year = vehicle.Year,
                Registration = vehicle.Registration,
                ListedAt = DateTimeOffset.UtcNow
            };
            bus.PubSub.Publish(message);
        }

        // PUT api/vehicles/ABC123
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] VehicleDto dto) {
            var vehicleModel = db.FindModel(dto.ModelCode);
            var vehicle = new Vehicle {
                Registration = dto.Registration,
                Color = dto.Color,
                Year = dto.Year,
                ModelCode = vehicleModel.Code
            };
            db.UpdateVehicle(vehicle);
            return Ok(dto);
        }

        // DELETE api/vehicles/ABC123
        [HttpDelete("{id}")]
        public IActionResult Delete(string id) {
            var vehicle = db.FindVehicle(id);
            if (vehicle == default) return NotFound();
            db.DeleteVehicle(vehicle);
            return NoContent();
        }
    }
}
