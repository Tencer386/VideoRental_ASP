using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using VR_FE_API.Models;
using VR_FE_API.ViewModels;

namespace VR_FE_API.Controllers
{
    public class RentalController : Controller
    {
        // GET: Rental
        public ActionResult Index()
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("Rentals").Result;

            IEnumerable<Rental> rentals = response.Content.ReadAsAsync<IEnumerable<Rental>>().Result;

            // get customer records
            response = WebClient.ApiClient.GetAsync("Customers").Result;
            IList<Customer> customers = response.Content.ReadAsAsync<IList<Customer>>().Result;

            // populate our CustomerRentalsViewModel
            var customerRentalsViewModel = rentals.Select(r => new CustomerRentalsViewModel
            {
                RentalId = r.RentalId,
                DateRented = r.DateRented,
                CustomerName = customers.Where(c => c.CustomerId == r.CustomerId).Select(u => u.CustomerName).FirstOrDefault(),
                DateReturned = r.DateReturned

            }).OrderByDescending(o => o.DateRented).ToList();


            return View(customerRentalsViewModel);
        }

        // GET: Rental/Details/5
        public ActionResult Details(int id)
        {
            HttpResponseMessage respons = WebClient.ApiClient.GetAsync($"Rentals/{id}").Result;
            var rental = respons.Content.ReadAsAsync<Rental>().Result;
            // get the customers record
            respons = WebClient.ApiClient.GetAsync("customers").Result;
            IList<Customer> customers = respons.Content.ReadAsAsync<IList<Customer>>().Result;

            // get the Movies record
            respons = WebClient.ApiClient.GetAsync("Movies").Result;
            IList<Movie> movies = respons.Content.ReadAsAsync<IList<Movie>>().Result;

            var customerRentalDetails = new CustomerRentalDetails
            {
                Rental = rental,
                CustomerName = customers.Where(c => c.CustomerId == rental.CustomerId).Select(u => u.CustomerName).FirstOrDefault(),
                RentedMovies = rental.RentalItems.Select(
                    ri => new CustomerMoviesViewModel
                    {
                        RentalItemId = ri.RentalItemId,
                        RentalId = ri.RentalId,
                        MovieName = movies.Where(m => m.MovieId == ri.MovieId).Select(n => n.Name).FirstOrDefault()
                    }).ToList()
            };

            return View(customerRentalDetails);
        }

        // GET: Rental/Create
        public ActionResult Create()
        {
            var rental = new Rental();
            rental.RentalId = -999;
            rental.DateRented = DateTime.Now;
            rental.Customers = GetCustomers();

            return View(rental);
        }

        // POST: Rental/Create
        [HttpPost]
        public ActionResult Create(Rental rental)
        {
            try
            {
                HttpResponseMessage respons = WebClient.ApiClient.PostAsJsonAsync("Rentals", rental).Result;
                rental = respons.Content.ReadAsAsync<Rental>().Result;
                respons = WebClient.ApiClient.GetAsync($"RentalItemsByID/{rental.RentalId}").Result;
                IList<RentalItem> rentalItems = respons.Content.ReadAsAsync<IList<RentalItem>>().Result;

                if (rentalItems.Count == 0)
                    return RedirectToAction("Edit", new { Id = rental.RentalId });
                else
                    return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        // GET: Rental/Edit/5
        public ActionResult Edit(int id)
        {
            HttpResponseMessage respons = WebClient.ApiClient.GetAsync($"Rentals/{id}").Result;
            var rental = respons.Content.ReadAsAsync<Rental>().Result;

            respons = WebClient.ApiClient.GetAsync($"RentalItemsById/{id}").Result;
            IList<RentalItem> rentalItems = respons.Content.ReadAsAsync<IList<RentalItem>>().Result;
            respons = WebClient.ApiClient.GetAsync("Movies").Result;
            IList<Movie> movies = respons.Content.ReadAsAsync<IList<Movie>>().Result;

            rental.Customers = GetCustomers();

            var rentedMovies = rentalItems.Select(ri => new CustomerMoviesViewModel
            {
                RentalItemId = ri.RentalItemId,
                RentalId = ri.RentalId,
                MovieName = movies.Where(mbox => mbox.MovieId == ri.MovieId).Select(r => r.Name).FirstOrDefault()
            }).ToList();

            rental.RentedMovies = rentedMovies;

            return View(rental);
        }

        // POST: Rental/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Rental rental)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.PutAsJsonAsync($"Rentals/{id}", rental).Result;

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                return View(rental);
            }
            catch
            {
                return View();
            }
        }

        // GET: Rental/Delete/5
        public ActionResult Delete(int id)
        {
            HttpResponseMessage respons = WebClient.ApiClient.GetAsync($"Rentals/{id}").Result;
            var rental = respons.Content.ReadAsAsync<Rental>().Result;
            // get the customers record
            respons = WebClient.ApiClient.GetAsync("customers").Result;
            IList<Customer> customers = respons.Content.ReadAsAsync<IList<Customer>>().Result;

            // get the Movies record
            respons = WebClient.ApiClient.GetAsync("Movies").Result;
            IList<Movie> movies = respons.Content.ReadAsAsync<IList<Movie>>().Result;

            var customerRentalDetails = new CustomerRentalDetails
            {
                Rental = rental,
                CustomerName = customers.Where(c => c.CustomerId == rental.CustomerId).Select(u => u.CustomerName).FirstOrDefault(),
                RentedMovies = rental.RentalItems.Select(
                    ri => new CustomerMoviesViewModel
                    {
                        RentalItemId = ri.RentalItemId,
                        RentalId = ri.RentalId,
                        MovieName = movies.Where(m => m.MovieId == ri.MovieId).Select(n => n.Name).FirstOrDefault()
                    }).ToList()
            };

            return View(customerRentalDetails);
        }

        // POST: Rental/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.DeleteAsync($"Rentals/{id}").Result;

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AddMovies(int RentalId)
        {
            var rentalItem = new RentalItem();
            rentalItem.RentalId = RentalId;
            rentalItem.Movies = GetMovies();

            return View(rentalItem);
        }

        [HttpPost]
        public ActionResult AddMovies(RentalItem rentalItem)
        {
            try
            {
                int Id = rentalItem.RentalId;
                HttpResponseMessage response = WebClient.ApiClient.PostAsJsonAsync("RentalItems", rentalItem).Result;

                return RedirectToAction("Edit", new { id = Id });
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EditRentedMovie(int Id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"RentalItems/{Id}").Result;
            var rentalItem = response.Content.ReadAsAsync<RentalItem>().Result;
            rentalItem.Movies = GetMovies();

            return View(rentalItem);
        }

        [HttpPost]
        public ActionResult EditRentedMovie(int Id, RentalItem rentalItem)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.PutAsJsonAsync($"RentalItems/{Id}", rentalItem).Result;
                Id = rentalItem.RentalId;
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Edit", new { id = Id });

                return View(rentalItem);
            }
            catch
            {
                return View();
            }
        }

        // Delete - Get
        public ActionResult DeleteRentedMovie(int Id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"RentalItems/{Id}").Result;
            var rentalItem = response.Content.ReadAsAsync<RentalItem>().Result;
            rentalItem.Movies = GetMovies();

            return View(rentalItem);
        }

        // Delete - Post

        [HttpPost]
        public ActionResult DeleteRentedMovie(int Id, FormCollection collection)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.DeleteAsync($"RentalItems/{Id}").Result;
                var rentalItem = response.Content.ReadAsAsync<RentalItem>().Result;

                return RedirectToAction("Edit", new { Id = rentalItem.RentalId });
            }
            catch
            {
                return View();
            }
        }

        #region Helper Methods

        public IEnumerable<SelectListItem> GetCustomers()
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("Customers").Result;
            IList<Customer> customerslist = response.Content.ReadAsAsync<IList<Customer>>().Result;
            List<SelectListItem> customers = customerslist.OrderBy(o => o.CustomerName).Select(c => new SelectListItem
            {
                Value = c.CustomerId.ToString(),
                Text = c.CustomerName
            }).ToList();
            return new SelectList(customers, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetMovies()
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("Movies").Result;
            IList<Movie> Movielist = response.Content.ReadAsAsync<IList<Movie>>().Result;
            List<SelectListItem> movies = Movielist.OrderBy(o => o.Name).Select(c => new SelectListItem
            {
                Value = c.MovieId.ToString(),
                Text = c.Name
            }).ToList();
            return new SelectList(movies, "Value", "Text");
        }

        #endregion
    }
}
