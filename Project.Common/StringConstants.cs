namespace Project.Common
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

        //Action names string representation
        public const string ActionNameIndex = "Index";
        public const string ActionNameLogin = "Login";
        public const string ActionNameLogout = "Logout";
        public const string ActionNameCreateAdministrator = "create-admin";
        public const string ActionNameCreateCustomer = "create-customer";
        public const string ActionNameCreateTechnician = "create-technician";

        //Controller names string representation
        public const string HomeControllerName = "Home";
        public const string UserControllerName = "User";
        public const string AccountControllerName = "Account";

        //Areas names stirng representation
        public const string AreaNameAdministration = "Administration";
        public const string AreaNameCustomer = "Customer";
        public const string AreaNameTechnician = "Technician";

        //User's roles names
        public const string AdminUserRole = "Admin";
        public const string CustomerUserRole = "Customer";
        public const string CorporateCustomerUserRole = "Corporate Customer";
        public const string TechnicianUserRole = "Technician";

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
