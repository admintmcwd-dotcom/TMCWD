using System;
using System.Collections.Generic;
using System.Text;
using TMCWD.Model.CustomerSupport.Interfaces;

namespace TMCWD.Model.CustomerSupport
{
    public class Request : IRequest
    {

        #region constructors

        public Request()
        {
            this.ControlNumber = string.Empty;
        }

        #endregion

        public int Id { get; set; }
        public string ControlNumber { get; set; }
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string CreateControlNumber()
        {
            throw new NotImplementedException();
        }
    }
}
