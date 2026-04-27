using System;
using System.Collections.Generic;
using System.Text;
using TMCWD.Model.Administrator.Interface;

namespace TMCWD.Model.Administrator
{
    public class InspectionType : IInspectionType
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
