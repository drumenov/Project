using Project.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Models.ViewModels.Administration
{
    public class OrderViewModel
    {
        public OrderViewModel() {
            this.OrderedParts = new HashSet<Part>();
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public ICollection<Part> OrderedParts { get; set; }
    }
}
