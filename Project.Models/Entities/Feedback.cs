using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models.Entities
{
    public class Feedback
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string UserId { get; set; }
        public virtual User Customer { get; set; }

        [ForeignKey(nameof(RepairTask))]
        public int RepairTaskId { get; set; }
        public virtual RepairTask RepairTask { get; set; }
    }
}
