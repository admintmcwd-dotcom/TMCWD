using System;
using System.Collections.Generic;
using System.Text;
using TMCWD.Model.CustomerSupport.Interfaces;

namespace TMCWD.Model.CustomerSupport
{
    public class Recommendation : IRecommendation
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int RecommendationUserId { get; set; }
        public string Recommendations { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
