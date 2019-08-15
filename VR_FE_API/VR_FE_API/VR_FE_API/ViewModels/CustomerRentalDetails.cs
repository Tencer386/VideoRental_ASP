using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VR_FE_API.Models;

namespace VR_FE_API.ViewModels
{
    public class CustomerRentalDetails
    {
        public Rental Rental { get; set; }
        public string CustomerName { get; set; }
        public List<CustomerMoviesViewModel> RentedMovies { get; set; }
    }
}