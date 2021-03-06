﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Project.Models.Entities
{
    public class User : IdentityUser
    {
        public User() {
            this.Orders = new HashSet<Order>();
            this.RepairTasks = new HashSet<UserRepairTask>();
            this.Receipts = new HashSet<Receipt>();
        }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<UserRepairTask> RepairTasks { get; set; }

        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}
