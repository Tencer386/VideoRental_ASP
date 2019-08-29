using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VR_FE_API.Models
{
    public class MovieRentalCountReport
    {
        public int MovieId { get; set; }
        public string Name { get; set; }
        public int? RentalCount { get; set; }
    }
}