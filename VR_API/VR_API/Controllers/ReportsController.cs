using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VR_API.Models;
using VR_API.ViewModels;

namespace VR_API.Controllers
{
    public class ReportsController : ApiController
    {
        private VideoRentalEntities db = new VideoRentalEntities();

        [HttpGet]
        [Route("api/Reports/GetMovieRentalCountReport")]
        public IEnumerable<MovieRentalCountReport> GetMovieRentalCountReport()
        {
            return db.MovieRentalCountReports;
        }

        [HttpGet]
        [Route("api/Reports/GetRentedMoviesReport")]
        public IEnumerable<RentedMovies> GetRentedMoviesReport()
        {
            string sql = "SELECT Rental.RentalId, Customer.CustomerName, Rental.DateRented, Movie.Name " +
                         "FROM Customer INNER JOIN " +
                         "Rental ON Customer.CustomerId = Rental.CustomerId INNER JOIN " +
                         "RentalItem ON Rental.RentalId = RentalItem.RentalId INNER JOIN " +
                         "Movie ON RentalItem.MovieId = Movie.MovieId " +
                         "ORDER BY Rental.DateRented DESC";

            return db.Database.SqlQuery<RentedMovies>(sql).ToList();
        }
    }
}
