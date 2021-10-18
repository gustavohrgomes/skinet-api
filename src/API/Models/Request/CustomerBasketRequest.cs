using API.Models.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Request
{
    public class CustomerBasketRequest
    {
        [Required]
        public string Id { get; set; }

        public List<BasketItemViewModel> Items { get; set; } = new List<BasketItemViewModel>();
    }
}