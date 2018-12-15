using Project.Models.Enums;

namespace Project.Models.Entities
{
    public class Part
    {
        public int Id { get; set; }

        public PartType Type { get; set; }

        public int Quantity { get; set; }
    }
}
