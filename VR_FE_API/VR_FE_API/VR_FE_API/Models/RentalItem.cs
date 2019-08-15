using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace VR_FE_API.Models
{
    public class RentalItem
    {
        public int RentalItemId { get; set; }
        public int RentalId { get; set; }
        public int MovieId { get; set; }
        public IEnumerable<SelectListItem> Movies { get; set; }
    }
}