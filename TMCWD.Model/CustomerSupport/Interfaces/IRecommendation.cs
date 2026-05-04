using System;
using System.Collections.Generic;
using System.Text;

namespace TMCWD.Model.CustomerSupport.Interfaces
{
    public interface IRecommendation
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int RecommendationUserId { get; set; }
        public string Recommendations { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
