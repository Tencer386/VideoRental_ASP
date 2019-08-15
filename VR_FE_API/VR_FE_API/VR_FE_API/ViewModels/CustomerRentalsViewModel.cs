using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VR_FE_API.ViewModels
{
    public class CustomerRentalsViewModel
    {
        public int RentalId { get; set; }
        public string CustomerName { get; set; }
        public DateTime DateRented { get; set; }
        public DateTime? DateReturned { get; set; }
    }
}