using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Saas.Api.Model
{
    public class UserModel
    {
        public Guid? UserId { get; set; }
        public Guid? PaymentPlanId { get; set; }
        [Required]
        public required string Key { get; set; }
    }
}