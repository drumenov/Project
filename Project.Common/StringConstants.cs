﻿namespace Project.Common
{
    public static class StringConstants
    {
        //TempData keys
        public const string TempDataKeyHoldingUserRole = "UserRole";
        public const string TempDataKeyHoldingNumberOfMaximumPagesForAdministrators = "MaximumPagesForAdministrators";
        public const string TempDataKeyHoldingThePageNumberForAdministrators = "PageForAdministrators";
        public const string TempDataKeyHoldingNumberOfMaximumPagesForCustomers = "MaximumPagesForCustomers";
        public const string TempDataKeyHoldingThePageNumberForCustomers = "PageForCustomers";
        public const string TempDataKeyHoldingNumberOfMaximumPagesForTechnicians = "MaximumPagesForTechnicians";
        public const string TempDataKeyHoldingThePageNumberForTechnicians = "PageForTechnicians";

        //Error messages
        public const string WrongUsernameOrPasswordErrorMessage = "Wrong Username or Password";
        public const string WrongAmountOfOrderedPartSelected = "You have selected a part to order, but the selected amount must be greater than zero.";
        public const string WrongAmountOfUnorderedPartSelected = "You have selected an amount for a part that you have not selected to order.";
        public const string WrongOrder = "You are trying to create an order without any parts.";

        //Action names string representation
        public const string ActionNameIndex = "index";
        public const string ActionNameLogin = "login";
        public const string ActionNameLogout = "logout";
        public const string ActionNameCreateAdministrator = "create-admin";
        public const string ActionNameCreateCustomer = "create-customer";
        public const string ActionNameCreateTechnician = "create-technician";
        public const string ActionNameOrderPart = "order";
        public const string ActionNameOrderDetails = "order-details";

        //Controller names string representation
        public const string HomeControllerName = "Home";
        public const string UserControllerName = "User";
        public const string AccountControllerName = "Account";
        public const string PartControllerName = "Part";
        public const string OrderControllerName = "Order";

        //Areas names stirng representation
        public const string AreaNameAdministration = "Administration";
        public const string AreaNameCustomer = "Customer";
        public const string AreaNameTechnician = "Technician";

        //User's roles names
        public const string AdminUserRole = "Admin";
        public const string CustomerUserRole = "Customer";
        public const string CorporateCustomerUserRole = "Corporate Customer";
        public const string NoviceTechnicianUserRole = "NoviceTechnician";
        public const string AverageTechnicianUserRole = "AverageTechnician";
        public const string AdvancedTechnicianUserRole = "AdvancedTechnician";
        public const string ExpertTechnicianUserRole = "ExpertTechnician";
        public const string TechnicianUserRole = "Technician"; /*This is used when decideng whether the user is an admin, a customer or a technician 
                                                                for the initial login process. Also for proper naming for the technicians roles 
                                                                (i.e. when the technician is a novice, the role is NoviceTechnician - a simple concatenation).*/

        //Custom View names
        public const string CreateAdministratorViewName = "create-admin";
        public const string CreateCustomerViewName = "create-customer";
        public const string CreateTechnicianViewName = "create-technician";

        //View Components names
        public const string ViewComponentAdministratorsName = "Administrators";
        public const string ViewComponentCustomersName = "Customers";
        public const string ViewComponentTechniciansName = "Technicians";
    }
}
