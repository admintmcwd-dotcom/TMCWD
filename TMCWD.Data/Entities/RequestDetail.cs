using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMCWD.Data.Entities
{

    [Table("request_details")]
    public class RequestDetail
    {

        #region constructors

        public RequestDetail() { }

        #endregion

        #region properties

        [Key, Column("Id")]
        public System.Int64 Id { get; set; }
        [Required, Column("RequestId")]
        public int RequestId { get; set; }
        [Required, Column("RequesTypeId")]
        public int RequestTypeId { get; set; }
        [MaxLength(255), Column("InspectionTypeDetail")]
        public string InspectionTypeDetail { get; set; }
        [Column("DateCreated")]
        public DateTime DateCreated { get; set; }
        [Key, Column("DateUpdated")]
        public DateTime DateUpdated { get; set; }

        #endregion

    }
}
