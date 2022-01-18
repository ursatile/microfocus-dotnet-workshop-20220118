using System;

namespace Autobarn.Messages {
    public class NewVehicleMessage {
        public string Registration { get; set; }
        public string Color { get; set; }
        public int Year { get; set; }
        public string ModelName { get; set; }
        public string ManufacturerName { get; set; }
        public DateTimeOffset ListedAt { get; set; }

        public NewVehiclePriceMessage ToPriceMessage(int price, string currencyCode) {
            return new NewVehiclePriceMessage {
                Color = this.Color,
                Year = this.Year,
                Registration = this.Registration,
                ModelName = this.ModelName,
                ManufacturerName = this.ManufacturerName,
                Price = price,
                CurrencyCode = currencyCode
            };
        }
    }

    public class NewVehiclePriceMessage : NewVehicleMessage {
        public int Price { get; set; }
        public string CurrencyCode { get; set; }
    }
}
