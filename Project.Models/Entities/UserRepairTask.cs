namespace Project.Models.Entities
{
    public class UserRepairTask
    {
        public string UserId { get; set; }
        public virtual User Expert { get; set; }

        public int RepairTaskId { get; set; }
        public virtual RepairTask RepairTask { get; set; }
    }
}
