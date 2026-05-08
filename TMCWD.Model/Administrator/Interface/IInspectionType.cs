using System;
using System.Collections.Generic;
using System.Text;

namespace TMCWD.Model.Administrator.Interface
{
    public interface IInspectionType
    {

        #region properties

        public decimal Id { get; set; }
        public string Name { get; set; }
        public bool WithDetail { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public int UpdatedBy { get; set; }

        #endregion

    }
}
