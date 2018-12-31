using System.ComponentModel.DataAnnotations;

namespace Project.Models.InputModels.Customer
{
    public class FeedbackInputModel
    {

        [Required]
        public int RepairTaskId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
