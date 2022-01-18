using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Autobarn.PricingEngine;

namespace Autobarn.PricingServer {
    public class PricerService : Pricer.PricerBase {
        private readonly ILogger<PricerService> _logger;
        public PricerService(ILogger<PricerService> logger) {
            _logger = logger;
        }

        public override Task<PriceReply> GetPrice(PriceRequest request, ServerCallContext context) {
            if (request.Color.Equals("blue", StringComparison.InvariantCultureIgnoreCase)) {
                return Task.FromResult(new PriceReply {
                    Price = 5000,
                    CurrencyCode = "USD"
                });
            } else if (request.Year < 1972) {
                return Task.FromResult(new PriceReply {
                    Price = 200,
                    CurrencyCode = "GBP"
                });
            } else {
                return Task.FromResult(new PriceReply {
                    Price = 12345,
                    CurrencyCode = "EUR"
                });
            }
        }
    }
}
