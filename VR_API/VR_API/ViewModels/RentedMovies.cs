using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VR_API.ViewModels
{
    public class RentedMovies
    {
        public int RentalId { get; set; }
        public string Name { get; set; }
        public string CustomerName { get; set; }
        public DateTime DateRented { get; set; }
    }
}