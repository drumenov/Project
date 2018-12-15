namespace Project.Common
{
    public static class StringConstants
    {
        //TempData keys
        public const string TempDataKeyHoldingUserRole = "UserRole";

        //Error messages
        public const string WrongUsernameOrPasswordErrorMessage = "Wrong Username or Password";

        //Action names string representation
        public const string ActionNameIndex = "Index";
        public const string ActionNameLogin = "Login";
        public const string ActionNameLogout = "Logout";
        public const string ActionNameCreateAdministrator = "create-admin";

        //Controller names string representation
        public const string HomeControllerName = "Home";
        public const string UserControllerName = "User";

        //Areas names stirng representation
        public const string AreaNameAdministration = "Administration";
        public const string AreaNameCustomer = "Customer";
        public const string AreaNameTechnician = "Technician";

        //User's roles names
        public const string AdminUserRole = "Admin";
        public const string CustomerUserRole = "Customer";
        public const string TechnicianUserRole = "Technician";
    }
}
