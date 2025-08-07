using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Shopularity.Ordering.OrderLines
{
    public class OrderLineUpdateDto
    {
        public Guid OrderId { get; set; }
        [Required]
        public string ProductId { get; set; } = null!;
        [StringLength(OrderLineConsts.NameMaxLength)]
        public string? Name { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public double TotalPrice { get; set; }

    }
}