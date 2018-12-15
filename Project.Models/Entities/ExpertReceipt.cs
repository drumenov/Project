namespace Project.Models.Entities
{
    public class ExpertReceipt
    {
        public string UserId { get; set; }
        public virtual User Expert { get; set; }

        public int ReceiptId { get; set; }
        public virtual Receipt Receipt { get; set; }
    }
}
