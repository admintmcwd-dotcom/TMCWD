namespace TMCWD.Data.Entities
{
    public class ReadingSheet
    {

        public System.Int64 Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime BillingDate { get; set; }

        public int Zone { get; set; }

        public int Book { get; set; }

        public System.Int64 AssignedTo { get; set; }

        public System.Int64 CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public System.Int64 UpdatedBy { get; set; }

        public DateTime DateUpdated { get; set; }

    }
}
