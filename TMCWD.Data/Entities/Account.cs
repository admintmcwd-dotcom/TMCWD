using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMCWD.Data.Entities
{

    [Table("accounts")]
    public class Account
    {

        #region constructors

        public Account() { }

        #endregion

        #region properties

        [Key, Column("Id")]
        public System.Int64 Id { get; set; }

        [Required, Column("CustomerId")]
        public System.Int64 CustomerId { get; set; }

        [Required, MaxLength(50), Column("AccountNumber")]
        public string AccountNumber { get; set; } = string.Empty;

        [Required, MaxLength(50), Column("MeterNumber")]
        public string MeterNumber { get; set; } = string.Empty;

        [Required, MaxLength(255), Column("Address")]
        public string Address { get; set; } = string.Empty;

        [Required, Column("IsCurrentAddress")]
        public bool IsCurrentAddress { get; set; }

        [Required, Column("IsActive")]
        public bool IsActive { get; set; }

        [Column("DateCreated")]
        public DateTime DateCreated { get; set; }

        [Column("DateUpdated")]
        public DateTime DateUpdated { get; set; }

        #endregion

    }
}
