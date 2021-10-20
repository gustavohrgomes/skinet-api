using API.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Request
{
    public class OrderRequest
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressResponse ShipToAddress { get; set; }
    }
}
