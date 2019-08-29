using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using VR_FE_API.Models;

namespace VR_FE_API.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult GetRentalCountData()
        {
            IEnumerable<MovieRentalCountReport> movieRentalCountReports = GetMovieRentalCountReport();
            return View(movieRentalCountReports.OrderBy(m => m.Name));
        }

        private IEnumerable<MovieRentalCountReport> GetMovieRentalCountReport()
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("Reports/GetMovieRentalCountReport").Result;
            IEnumerable<MovieRentalCountReport> movieRentalCountReports = response.Content.ReadAsAsync<IEnumerable<MovieRentalCountReport>>().Result;

            return movieRentalCountReports;
        }

        public ActionResult DrawMovieRentalCountChart()
        {
            IEnumerable<MovieRentalCountReport> movieRentalCountReports = GetMovieRentalCountReport();
            return View(movieRentalCountReports);
        }

        [HttpGet]
        public ActionResult GetRentedMovieReport(string criteria)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("Reports/GetRentedMoviesReport").Result;
            IEnumerable<RentedMoviesViewModel> rentedMovieReport = response.Content.ReadAsAsync<IEnumerable<RentedMoviesViewModel>>().Result;

            if (string.IsNullOrEmpty(criteria))
            {
                TempData["RentedMovies"] = rentedMovieReport;
                return View(rentedMovieReport);
            }
            else
            {
                rentedMovieReport = rentedMovieReport.Where(m => m.Name.ToLower().Contains(criteria.ToLower()) || m.CustomerName.ToLower().Contains(criteria.ToLower())).ToList();
                TempData["RentedMovies"] = rentedMovieReport;
                return View(rentedMovieReport);
            }
        }

        public void ExportRentedMovieData()
        {
            List<RentedMoviesViewModel> rentedMovies = TempData["RentedMovies"] as List<RentedMoviesViewModel>;

            StringWriter sw = new StringWriter();
            sw.WriteLine("\"RentalId\", \"Name\",\"CustomerName\",\"DateRented\"");
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=RentedMovies.csv");
            Response.ContentType = "application/octet-stream";

            foreach(var rentedMovie in rentedMovies)
            {
                sw.WriteLine($"{rentedMovie.RentalId}, {rentedMovie.Name}, {rentedMovie.CustomerName}, {rentedMovie.DateRented}"); 
            }
            Response.Write(sw.ToString());
            Response.End();
        }
    }
}