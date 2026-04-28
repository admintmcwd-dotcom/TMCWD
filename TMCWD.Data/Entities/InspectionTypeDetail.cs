using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMCWD.Data.Entities
{
    [Table("inspection_type_details")]
    public class InspectionTypeDetail
    {

        #region constructors

        public InspectionTypeDetail() 
        {
            this.Detail = string.Empty;
        }

        #endregion

        #region properties

        [Key]
        public System.Int64 Id { get; set; }

        [Required]
        public System.Int64 InspectionTypeId { get; set; }

        [Required, MaxLength(255)]
        public string Detail { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        #endregion

    }
}
