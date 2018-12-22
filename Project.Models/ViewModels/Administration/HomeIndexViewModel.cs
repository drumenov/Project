using System.Collections.Generic;

namespace Project.Models.ViewModels.Administration
{
    public class HomeIndexViewModel
    {
        public IList<AdministratorViewModel> Administrators { get; set; }

        public IList<CustomerViewModel> Customers { get; set; }

        public IList<TechnicianViewModel> Technicians { get; set; }
    }
}
