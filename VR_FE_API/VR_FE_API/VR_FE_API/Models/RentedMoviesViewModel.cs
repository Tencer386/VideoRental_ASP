using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VR_FE_API.Models
{
    public class RentedMoviesViewModel
    {
        public int RentalId { get; set; }
        public string Name { get; set; }
        public string CustomerName { get; set; }
        public DateTime DateRented { get; set; }
    }
}