using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VR_FE_API.ViewModels;

namespace VR_FE_API.Models
{
    public class Rental
    {
        public int RentalId { get; set; }
        public int CustomerId { get; set; }
        public DateTime DateRented { get; set; }
        public DateTime? DateReturned { get; set; }
        public virtual ICollection<RentalItem> RentalItems { get; set; }
        public IEnumerable<SelectListItem> Customers { get; set; }
        public IEnumerable<CustomerMoviesViewModel> RentedMovies { get; set; }
    }
}