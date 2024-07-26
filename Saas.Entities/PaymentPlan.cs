using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Saas.Entities.enums;

namespace Saas.Entities
{
    public class PaymentPlan : BaseEntity
    {
        public required string Name { get; set; }
        public decimal MonthlyCost { get; set; }
        public string? Description { get; set; }
        public PaymentPlanType Type { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Organization>? Organizations { get; set; }
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}