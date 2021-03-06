﻿using System.Collections.Generic;

namespace Project.Models.Entities
{
    public class Order
    {
        public Order() {
            this.OrderedParts = new HashSet<Part>();
        }

        public int Id { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Part> OrderedParts { get; set; }
    }
}
