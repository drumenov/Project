namespace Project.Common.Constants
{
    public static class StringConstants
    {
        //TempData keys
        public const string TempDataKeyHoldingUserRole = "UserRole";
        //public const string TempDataKeyHoldingNumberOfMaximumPagesForAdministrators = "MaximumPagesForAdministrators";
        public const string TempDataKeyHoldingThePageNumberForAdministrators = "PageForAdministrators";
        //public const string TempDataKeyHoldingNumberOfMaximumPagesForCustomers = "MaximumPagesForCustomers";
        public const string TempDataKeyHoldingThePageNumberForCustomers = "PageForCustomers";
        //public const string TempDataKeyHoldingNumberOfMaximumPagesForTechnicians = "MaximumPagesForTechnicians";
        public const string TempDataKeyHoldingThePageNumberForTechnicians = "PageForTechnicians";
        public const string TempDataKeyHoldingThePageNumberForPendingRepairTasks = "PageForPendingRepairTasks";
        public const string TempDataKeyHoldingThePageForWorkedOnRepairTasks = "PageForWorkedOnRepairTasks";
        public const string TempDataKeyHoldingThePageForWorkedOnRepairTasksByTechnician = "PageForWorkedOnRepairTasksByTechnician";
        public const string TempDataKeyHoldingThePageForFinishedRepairTasks = "PageForFinishedRepairTasks";
        public const string TempDataKeyHoldingThePageForFinishedRepairTasksByTechnician = "PageForFinishedRepairTasksByTechnician";
        public const string TempDataKeyHoldingThePageForPendingRepairTasksPerCustomer = "PageForPendingRepairTasksPerCustomer";
        public const string TempDataKeyHoldingThePageForWorkedOnRepairTasksPerCustomer = "PageForWorkedOnRepairTasksPerCustomer";
        public const string TempDataKeyHoldingThePageforFinishedRepairTasksPerCustomer = "PageForFinishedRepairTasksPerCustomer";
        public const string TempDataKeyHoldingGenericErrorsForRepairTaskController = "RepairTaskControllerError";
        public const string TempDataKeyHoldingGenericError = "error";

        //Error messages
        public const string WrongUsernameOrPasswordErrorMessage = "Wrong Username or Password";
        public const string WrongAmountOfOrderedPartSelected = "You have selected a part to order, but the selected amount must be greater than zero.";
        public const string WrongAmountOfUnorderedPartSelected = "You have selected an amount for a part that you have not selected to order.";
        public const string WrongOrder = "You are trying to create an order without any parts.";
        public const string WrongRepairTask = "You are trying to order some repairs, but you have not selected which parts are to be repaired.";
        public const string NotEnoughPartsAvailable = "The amount of additional needed parts of type {0} is {1}";
        public const string RepairTaskGenericAssignmentFailure = "Either not enough parts are available or someone else has already assign this task.";
        public const string WrongInputWhenAddingOrRemovingTechniciansFromRepairTask = "You must remove or add at least one technician.";
        public const string WrongInputWhenPromotingATechnician = "Technician cannot be promoted due to wrong input";
        public const string RemovingOldTechnicianLevelError = "An error occured while trying to remove the old Technician level.";
        public const string AddingNewTechnicianLevelError = "An error occured whilte trying to add the new Technician level.";
        public const string CreatingANewTechnicianLevelError = "An error occured while creating the new Technician level.";
        public const string TryingToDemoteTechnicianThatCannotBeDemotedError = "You cannot demote this Technician";
        public const string TryingToPromoteATechnicianThatCannotBePromotedError = "You cannot promote this Technician";
        public const string TryingToRetrieveAMissingReceiptError = "Receipt with this ID: {0} does not exists";
        public const string NoRepairTaskWithGivenIdError = "A repair task with ID {0} does not exist";
        public const string NoChangedInRepairTaskError = "Nothing was changed in the Repair Task";
        public const string RepairTaskChangesNotPossibleError = "You cannot make these changes.";
        public const string WrongAmountOfPartTypeToUpdate = "You have selected a Part Type to update but the amount must be at least zero (to delete this Part from the Repair Task).";
        public const string AmountForNotSelectedPartTypeToUpdateError = "You have not selected the appropriate Part Type.";
        public const string NoChangesWhenEditingFeedbackError = "You have not changed anything in the feedback.";


        //Action names string representation
        public const string ActionNameIndex = "index";
        public const string ActionNameLogin = "login";
        public const string ActionNameLogout = "logout";
        public const string ActionNameCreateAdministrator = "create-admin";
        public const string ActionNameCreateCustomer = "create-customer";
        public const string ActionNameCreateTechnician = "create-technician";
        public const string ActionNameOrderPart = "order";
        public const string ActionNameRepairTaskDetails = "repair-task-details";
        //public const string ActionNameOrderDetails = "order-details";
        public const string ActionNameCreateRepairTask = "create-repair-task";
        public const string ActionNameAssignTask = "assign-repair-task";
        public const string ActionNameDoMagic = "technician-does-magic";
        public const string ActionNameAllOrders = "all-orders";
        public const string ActionaNameAddRemoveTechnicians = "add-or-remove-technicians";
        public const string ActionNameCustomerDetails = "customer-details";
        public const string ActionNameRepairTaskReceipt = "repair-task-receipt";
        public const string ActionNamePromoteTechnician = "promote";
        public const string ActionNameDemoteTechnician = "demote";
        public const string ActionNameTechnicianDetails = "technician-details";
        public const string ActionNameAllReceipts = "all-receipts";
        public const string ActionNameReceiptDetails = "receipt-details";
        public const string ActionNameAllReparTaskPerCustomer = "all-repair-tasks";
        public const string ActionNameEditRepairTask = "edit-repair-task";
        public const string ActionNameDeleteRepairTask = "delete-repair-task";
        public const string ActionNameGiveFeedbackForARepairTask = "repair-task-feedback";
        public const string ActionNameEditFeedbackForARepairTask = "repair-task-edit-feedback";
        public const string ActionNameAllFeedbacks = "all-feedbacks";

        //Controller names string representation
        public const string HomeControllerName = "Home";
        public const string UserControllerName = "User";
        public const string AccountControllerName = "Account";
        public const string PartControllerName = "Part";
        public const string OrderControllerName = "Order";
        public const string RepairTaskControllerName = "RepairTask";
        public const string CustomerControllerName = "Customer";
        public const string TechnicianControllerName = "Technician";
        public const string ReceiptControllerName = "Receipt";
        public const string FeedbackControllerName = "Feedback";

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
        public const string UserRolesThatAreAuthorisedToUseTheCustomerArea = "Customer, Corporate Customer";
        public const string UserRolesThatAreAuthorisedToUseTheTechnicianArea = "NoviceTechnician, AverageTechnician, AdvancedTechnician, ExpertTechnician";

        //Custom View names
        public const string CreateAdministratorViewName = "create-admin";
        public const string CreateCustomerViewName = "create-customer";
        public const string CreateTechnicianViewName = "create-technician";

        //View Components names
        public const string ViewComponentAllAdministratorsNames = "Administrators";
        public const string ViewComponentAllCustomersNames = "Customers";
        public const string ViewComponentAllTechniciansNames = "Technicians";
        public const string ViewComponentAllPendingRepairTasks = "AllPendingRepairTasks";
        public const string ViewComponentWorkedOnRepairsTaskByTechnician = "WorkedOnRepairTasksByTechnician";
        public const string ViewComponentAllWorkedOnRepairTasks = "AllWorkedOnRepairTasks";
        public const string ViewComponentAllFinishedRepairTasks = "AllFinishedRepairTasks";
        public const string ViewComponentFinishedRepairTasksByTechnician = "TechnicianFinishedRepairTasks";
        public const string ViewComponentCustomerPendingRepairTasks = "ACustomerPendingRepairTasks";
        public const string ViewComponentCustomerWorkedOnRepairTasks = "ACustomerWorkedOnRepairTasks";
        public const string ViewComponentCustomerFinishedRepairTasks = "ACustomerFinishedRepairTasks";

        //View names
        public const string ViewForGivingAndEditingFeedback = "giveFeedback";
    }
}
