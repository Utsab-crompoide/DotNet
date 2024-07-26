using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Saas.Entities.enums;

namespace Saas.Entities
{
    public class Payment : BaseEntity
    {
        public Guid PlanId { get; set; }
        public Guid InitiatingUserId { get; set; }
        public PaymentMode Mode { get; set; }
        public PaymentStatus Status { get; set; }
        public Guid? VerifyingUserId { get; set; }
        public PaymentPlan? Plan { get; set; }
        public User? InitiatingUser { get; set; }
        public User? VerifyingUser { get; set; }
    }
}